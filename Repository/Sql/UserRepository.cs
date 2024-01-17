using Facebook.Data;
using Facebook.DTO;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Facebook.Repository.Sql
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<List<User>> GetFriendsAsync(int userId)
        {

            var  friends = await _context.Friendships
                .Where(f => (f.User1ID == userId || f.User2ID == userId) && f.Status == "Active")
                .Select(f => f.User1ID == userId ? f.User2 : f.User1)
                .ToListAsync();

            if (friends != null && friends.Any())
            {
                return friends.Select(u => u!).ToList(); // Dodaj oznaku "!" kako bi obavestio kompajler da je vrednost ne-null
            }
            return new List<User>();
        }

        public async Task<User> RegisterAsync(User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            User newUser = new User(user.FirstName,user.LastName, user.Username,user.Email, hashedPassword, DateTime.Now);

            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<bool> UpdateProfileAsync(int id, User user)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
            {
                // Korisnik nije pronađen, možete vratiti false ili neku drugu vrednost prema potrebi.
                return false;
            }

            existingUser.FirstName= user.FirstName;
            existingUser.LastName= user.LastName;
            existingUser.Email= user.Email;
            existingUser.Username= user.Username;
            existingUser.Password= BCrypt.Net.BCrypt.HashPassword(user.Password);
            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> AddFriendAsync(int userId, int friendId)
        {
            var user = await _context.Users.FindAsync(userId);
            var friend = await _context.Users.FindAsync(friendId);

            if (user == null || friend == null)
            {
                return false;
            }
            // Ako nisu, dodajte ih kao prijatelje

            if (user.Friendships != null && user.Friendships.Any(f => f.User2ID == friendId))
            {
                return true; // Već su prijatelji, ne treba ništa raditi
            }

            var newRequest = new FriendshipRequest(userId, friendId, "Pending");
            
             _context.FriendshipRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessFriendshipRequestAsync(int receiverId, int senderId, bool status)
        {
            var sender = await _context.Users.FindAsync(senderId);
            var receiver = await _context.Users.FindAsync(receiverId);

            if (sender == null || receiver == null)
            {
                return false; // users not found
            }

            var friendshipRequest = await _context.FriendshipRequests
                .FirstOrDefaultAsync(fr => fr.SenderID == senderId && fr.ReceiverID == receiverId);

            if (friendshipRequest == null)
            {
                return false; // Ako zahtev za prijateljstvom nije pronađen, obustavite operaciju
            }

            if (status)
            {
                // Ako je zahtev za prijateljstvom prihvaćen, dodaj u friendShip da su prijatelji i azuriraj u frinedShipRequest
                friendshipRequest.Status = "Accepted";
                _context.Friendships.Add(new Friendship(senderId, receiverId, "Active"));

            }
            else
            {
                _context.FriendshipRequests.Remove(friendshipRequest);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> LoginAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }

        public string GenerateJwtToken(User user)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                // Handle null or empty configuration values
                return null;
            }
            var keyBytes = Encoding.UTF8.GetBytes(key);

            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> RemoveFriendAsync(int userId, int friendId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeactivateProfile(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false; // user not found

             _context.Remove(user); //delete all profile

            await _context.SaveChangesAsync();

            return true; //succesful deleted
        }
    }
}
