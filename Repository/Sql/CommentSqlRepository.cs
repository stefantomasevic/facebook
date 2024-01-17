using Facebook.Data;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repository.Sql
{
    public class CommentSqlRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentSqlRepository(ApplicationDbContext context)
        {
            _context = context;   
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            _context.Comments.Add(comment);
             await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            var commentToDelete = await _context.Comments.FindAsync(commentId);

            if (commentToDelete == null)
                return false; // Comment not found

            _context.Comments.Remove(commentToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<string> EditCommentAsync(int commentId, string updatedComment)
        {
            try
            {
                var existingComment = await _context.Comments.FindAsync(commentId);

                if (existingComment == null)
                    return "Comment not found";

                existingComment.CommentDate = DateTime.Now;
                existingComment.CommentText = updatedComment;

                _context.Update(existingComment);

                await _context.SaveChangesAsync();

                return "Comment updated successfully";
            }
            catch (Exception ex)
            {
                // Log the exception for further investigation
                // You might want to log ex.Message, ex.StackTrace, etc.

                return "Internal server error";
            }
        }

        public Task<List<Comment>> GetAllCommentsAsync(int postId)
        {
            return _context.Comments.Where(c => c.PostID == postId).ToListAsync();
        }
    }
}
