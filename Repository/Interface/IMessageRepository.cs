using Facebook.Models;

namespace Facebook.Repository.Interface
{
    public interface IMessageRepository
    {

        Task<bool> AddMessage(Message message);

        Task<List<Message>> GetMessagesBetweenUsers(int user1Id, int user2Id);

        Task<bool> DeleteMessageAsync(int messageId);
    }
}
