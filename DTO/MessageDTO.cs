namespace Facebook.DTO
{
    public class MessageDTO
    {
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }


    }
}
