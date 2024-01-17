using Facebook.Models;

namespace Facebook.Repository.Interface
{
    public interface ICommentRepository
    {

        Task<List<Comment>> GetAllCommentsAsync(int postId);

        Task<Comment> AddCommentAsync(Comment comment);

        Task<string> EditCommentAsync(int commentId, string comment);

        Task<bool> DeleteCommentAsync(int commentId);
    }
}
