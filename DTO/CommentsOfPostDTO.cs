using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Facebook.DTO
{
    public class CommentsOfPostDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public int userId { get; set; }

        public int postId { get; set; }
    }
}
