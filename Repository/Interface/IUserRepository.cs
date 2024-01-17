using Facebook.Models;

namespace Facebook.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int userId);
        Task<List<User>> GetFriendsAsync(int userId);
        Task<bool> UpdateProfileAsync(int id, User user);

        Task<User> RegisterAsync(User user);

        Task<bool> AddFriendAsync(int userId, int friendId);
        Task<bool> RemoveFriendAsync(int userId, int friendId);

        Task<bool> ProcessFriendshipRequestAsync(int reciverId, int senderId, bool status);

        Task<User> LoginAsync(string username, string password);

        string GenerateJwtToken(User user);

        Task<bool> DeactivateProfile(int userId);


    }
}
