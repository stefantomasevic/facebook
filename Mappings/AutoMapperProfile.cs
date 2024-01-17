using AutoMapper;
using Facebook.DTO;
using Facebook.Models;

namespace Facebook.Mappings
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<RegistrationDTO, User>();


            CreateMap<PostManipulationDTO, Post>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) // Assuming ID is auto-generated
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => MapImage(src.Image)))
                .ForMember(dest => dest.PostDate, opt => opt.MapFrom(src => DateTime.Now));


            CreateMap<Post, PostWithUserInfoDTO>()
        .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));
            // Map IFormFile to string as needed

            CreateMap< Comment, CommentsOfPostDTO>()
                 .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.CommentDate))
                 .ForMember(dest => dest.userId, opt => opt.MapFrom(src => src.UserID))
                 .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.CommentText))
                 .ForMember(dest => dest.postId, opt => opt.MapFrom(src => src.PostID))
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID));

            CreateMap<MessageDTO, Message>()
           .ForMember(dest => dest.Image, opt => opt.MapFrom(src => SaveImage(src.Image)))
            .ForMember(dest => dest.SendingDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.MessageText, opt => opt.MapFrom(src => src.Text));


            CreateMap<CommentsOfPostDTO, Comment>()
                  .ForMember(dest => dest.CommentDate, opt => opt.MapFrom(src => src.Date))
                    .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.userId))
                    .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.Text))
                    .ForMember(dest => dest.PostID, opt => opt.MapFrom(src => src.postId))
                    .ForMember(dest => dest.ID, opt => opt.Ignore());

            CreateMap<User, FriendDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username));


        }

        private object MapImage(IFormFile? image) //pciture for posts
        {
            if (image != null)
            {
                // path for picture
                string folderPath = Path.Combine("Pictures", "Post");

                // if folder dont exist create 
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // generate guid for picture
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

                // path
                string filePath = Path.Combine(folderPath, uniqueFileName);

                // save picture in folder images/post on server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                // return path for database
                return filePath;
            }
            return null;
        }
        private string SaveImage(IFormFile? image)
        {
            if (image != null)
            {
                string folderPath = Path.Combine("Pictures", "Message");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(folderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }

                return filePath;
            }

            return null;
        }

    }
}

