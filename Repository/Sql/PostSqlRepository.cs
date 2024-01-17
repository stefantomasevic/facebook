using Facebook.Data;
using Facebook.DTO;
using Facebook.Models;
using Facebook.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Repository.Sql
{
    public class PostSqlRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostSqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddPost(Post post)
        {
            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Exception due add post", ex);
            }

        }

        public async  Task<List<PostWithUserInfoDTO>> GetFriendsPosts(int userId)
        {

            var friendsPosts = await _context.Posts
        .Where(p => _context.Friendships
            .Any(f =>
                (f.User1ID == userId && f.User2ID == p.UserID && f.Status == "Active") ||
                (f.User2ID == userId && f.User1ID == p.UserID && f.Status == "Active")
            )
        )
        .Join(
            _context.Users,
            post => post.UserID,
            user => user.ID,
            (post, user) => new PostWithUserInfoDTO
            {
                PostId = post.ID,
                PostText = post.PostText,
                Image = post.Image,
                PostDate = post.PostDate,
                UserId = post.UserID,
                UserName = user.Username
            }
        )
        .ToListAsync();
            return friendsPosts;


        }
        public async Task<bool> DeletePostAsync(int postId)
        {
            var postToDelete = await _context.Posts.FindAsync(postId);

            if (postToDelete == null)
            {
                // Post sa datim ID-om nije pronađen
                return false;
            }

            _context.Posts.Remove(postToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public  async Task<bool> UpdatePostAsync(int postId, Post updatedPost)
        {

            //if existing.userID==updatedPost.id just in that case can make change (that is your post)
            var existingPost = await _context.Posts.FindAsync(postId);
            if(existingPost != null)
            {
                existingPost.PostText = updatedPost.PostText;
            }

            if (existingPost == null)
            {
                return false; 
            }
            try
            {
                _context.Posts.Update(existingPost);
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception)
            {
                return false; 
            }
         
        }
    }
}
