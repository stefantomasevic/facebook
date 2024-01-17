namespace Facebook.DTO
{
    public class UserMessageDTO
    {
        public int SenderID { get; set; }
        public string SenderUsername { get; set; }
        public int ReceiverID { get; set; }
        public string ReceiverUsername { get; set; }
        public List<MessageDetailDTO> Messages { get; set; }
    }
}
