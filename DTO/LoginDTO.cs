namespace Facebook.DTO
{
    public class LoginDTO
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public LoginDTO(string username,string password)
        {
            UserName = username;
            Password = password;
        }
    }
}
