namespace Facebook.Models
{
    public class Comment
    {
        public int ID { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public int UserID { get; set; }
        public User? User { get; set; }
        public int PostID { get; set; }
        public Post? Post { get; set; }


        public Comment(string commentText, DateTime commentDate, int userID, int postID)
        {
            CommentText = commentText;
            CommentDate = commentDate;
            UserID = userID;
            PostID = postID;
        }

        public Comment()
        {
            
        }
    }
}
