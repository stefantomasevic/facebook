namespace Facebook.Models
{
    public class Friendship
    {
        public int ID { get; set; }
        public int User1ID { get; set; }
        public User? User1 { get; set; }
        public int User2ID { get; set; }

        public User? User2 { get; set; }
        public string Status { get; set; } // active


        public Friendship(int user1ID, int user2ID, string status)
        {
            User1ID = user1ID;
            User2ID = user2ID;
            Status = status;
        }
    }
}
