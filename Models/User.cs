
namespace Facebook.Models
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public DateTime RegistrationDate { get; set; }

        public List<Post>? Posts { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Friendship>? Friendships { get; set; }
        public List<Message>? SentMessages   { get; set; }
        public List<Message>? ReceivedMessages { get; set; }

        public User()
        {
            
        }

        public User( string firstName, string lastName, string username, string email, string password, DateTime registrationDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Username = username;
            Email = email;
            Password = password;
            RegistrationDate = registrationDate;
        }
    }
}
