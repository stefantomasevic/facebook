namespace Facebook.DTO
{
    public class PostWithUserInfoDTO
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public string Image { get; set; }
        public DateTime PostDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
