using AutoMapper;
using Facebook.DTO;
using Facebook.Hubs;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Facebook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;
        public UserController(IUserRepository repository,IMapper mapper,IHubContext<ChatHub> hubContext)
        {
            _repository = repository;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet("friends/{id}")] //all friends for one user by userID
        public async Task<IActionResult> GetFriends(int id)
        {
            var friends = await _repository.GetFriendsAsync(id);
            return Ok(_mapper.Map<List<FriendDTO>>(friends));
        } 
        [HttpPost("signup")]

        public async Task<IActionResult> SignUp(RegistrationDTO registrationDTO)
        {
            try
            {
                var user = await _repository.RegisterAsync(_mapper.Map<User>(registrationDTO));

                return Ok("Registration successful");
            }
            catch (Exception ex)
            {
 
                return BadRequest("Registration failed");
            }
        } //registry on network

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProfile(int id, RegistrationDTO registrationDTO)
        {
            var isUpdateSuccessful = await _repository.UpdateProfileAsync(id, _mapper.Map<User>(registrationDTO));

            if (!isUpdateSuccessful)
            {
                return NotFound(); // User not found
            }

            return Ok(); // Update succesfull
        }

        [HttpPost("add-friend/{friendId}")]
        public async Task<IActionResult> AddFriend(int userId, int friendId)
        {
            var result = await _repository.AddFriendAsync(userId, friendId);

            var recevier = await _repository.GetByIdAsync(friendId);
            var sender = await _repository.GetByIdAsync(userId);

            if (result && recevier!=null && sender!=null)
            {
                await _hubContext.Clients.User(recevier.Username).SendAsync("ReceiveFriendRequest", sender.Username);
                return Ok("Friend request sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send friend request. Please check user IDs and try again.");
            }
        }

        [HttpPut("process-friendship-request")]
        public async Task<IActionResult> ProcessFriendshipRequest(FriendShipRequestDTO friendShipRequestDTO)//accept or reject request
        {
            var result = await _repository.ProcessFriendshipRequestAsync(friendShipRequestDTO.ReciverID, friendShipRequestDTO.SenderID, friendShipRequestDTO.Status); 

            if (result)
            {
                return Ok(); // Uspešno procesuirano
            }

            return BadRequest(); // Greška prilikom procesuiranja zahteva
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _repository.LoginAsync(loginDTO.UserName, loginDTO.Password);

            if (user != null)
            {
                // Return user information or generate a token for authentication
                var token = _repository.GenerateJwtToken(user);
                return Ok(new { Token = token, Message = "Login successful" });
            }

            // Return a 401 Unauthorized status if login fails
            return Unauthorized("Invalid username or password");
        }

        [HttpDelete("deleteProfile/{userId}")]
        public async Task<IActionResult> DeactivateProfile(int userId)
        {
            var success = await _repository.DeactivateProfile(userId);

            if (success)
            {
                return Ok("Profile deleted");
            }
            else
            {
                return NotFound("User not found");
            }
        }

    }
}
