using Facebook.Models;

namespace Facebook.DTO
{
    public class PostManipulationDTO
    {
        public string PostText { get; set; }
        public IFormFile? Image { get; set; }
        public int UserID { get; set; }

        public PostManipulationDTO(string postText, IFormFile image, int userID)
        {
            PostText = postText;
            Image = image;
            UserID = userID;
        }

        public PostManipulationDTO()
        {
            // Inicijalizacija po potrebi
        }
    }
}
