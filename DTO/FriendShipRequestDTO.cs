namespace Facebook.DTO
{
    public class FriendShipRequestDTO
    {

        public int ReciverID { get; set; }

        public int SenderID { get; set; }

        public bool Status { get; set; } //true is accepted, false is rejected

        public FriendShipRequestDTO(int reciverId,int  senderId, bool status) 
        {
            ReciverID = reciverId;
            SenderID=senderId;
            Status = status;
        }
    }
}
