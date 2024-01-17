namespace Facebook.DTO
{
    public class MessageManipulationDTO
    {

        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }

        public DateTime Time { get; set; }

        public string SenderUsername { get; set; }
        public string ReceiverUsername { get; set; }
    }
}
