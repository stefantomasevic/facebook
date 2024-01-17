namespace Facebook.Models
{
    public class FriendshipRequest
    {

        public int ID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public string? Status { get; set; } // Status for example ( "Pending", "Accepted", "Rejected")
        public User? Sender { get; set; }
        public User? Receiver { get; set; }



        public FriendshipRequest(int senderID, int receiverID, string status)
        {
            SenderID = senderID;
            ReceiverID = receiverID;
            Status = status;
        }

    }
}
