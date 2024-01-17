namespace Facebook.DTO
{
    public class MessageDetailDTO
    {
        public string? Text { get; set; }
        public string  Image { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }

        public DateTime Time { get; set; }

    }
}
