using System.ComponentModel.DataAnnotations.Schema;

namespace Facebook.Models
{
    public class Post
    {
        public int ID { get; set; }
        public string PostText { get; set; }
        public string? Image { get; set; }
        public DateTime PostDate { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }

        public List<Comment>? Comments { get; set; }


        public Post(string postText, string image, DateTime postDate, int userID)
        {
            PostText = postText;
            Image = image;
            PostDate = postDate;
            UserID = userID;
        }

        public Post()
        {
            
        }

    }
}
