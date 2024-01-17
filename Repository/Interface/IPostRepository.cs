using Facebook.DTO;
using Facebook.Models;

namespace Facebook.Repository.Interface
{
    public interface IPostRepository
    {


        Task AddPost(Post post);

        Task<List<PostWithUserInfoDTO>> GetFriendsPosts(int userId);

        Task<bool> DeletePostAsync(int postId);

        Task<bool> UpdatePostAsync(int postId, Post updatedPost);

    }
}
