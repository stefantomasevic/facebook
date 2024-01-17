namespace Facebook.Models
{
    public class Message
    {

        public int ID { get; set; }
        public string? MessageText { get; set; }

        public string? Image { get; set; }
        public DateTime SendingDate { get; set; }
        public int SenderID { get; set; }
        public User? Sender { get; set; }
        public int ReceiverID { get; set; }
        public User? Receiver { get; set; }


        public Message(string messageText, DateTime sendingDate, int senderID, int receiverID, string image)
        {
            MessageText = messageText;
            SendingDate = sendingDate;
            SenderID = senderID;
            ReceiverID = receiverID;
            Image = image;
        }

        public Message()
        {
            
        }
    }
}
