using Facebook.Data;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repository.Sql
{
    public class MessageSqlRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageSqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMessageAsync(int messageId)
        {
            var messageToDelete = await _context.Messages.FindAsync(messageId);

            if (messageToDelete != null)
            {
                _context.Messages.Remove(messageToDelete);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public Task<List<Message>> GetMessagesBetweenUsers(int user1Id, int user2Id)
        {

                var messages = _context.Messages
                    .Where(m => (m.SenderID == user1Id && m.ReceiverID == user2Id) || (m.SenderID == user2Id && m.ReceiverID == user1Id))
                    .ToListAsync();
                return messages;

           
        }
    }
}
