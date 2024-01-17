using Facebook.Models;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Data
{
    public class ApplicationDbContext:DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
            Users = Users ?? Set<User>();
            Friendships= Friendships ?? Set<Friendship>();
            Comments= Comments?? Set<Comment>();
            FriendshipRequests = FriendshipRequests?? Set<FriendshipRequest>();
            Posts = Posts ?? Set<Post>();
            Messages = Messages ?? Set<Message>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<FriendshipRequest> FriendshipRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ostale konfiguracije...
            modelBuilder.Entity<Friendship>()
                   .HasOne(f => f.User1)
                   .WithMany(u => u.Friendships)
                   .HasForeignKey(f => f.User1ID)
                   .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User2)
                .WithMany() // Nema potrebe za drugim navigacionim svojstvom jer koristimo User1 za oba odnosa
                .HasForeignKey(f => f.User2ID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendshipRequest>()
                 .HasOne(fr => fr.Sender)
                 .WithMany()
                 .HasForeignKey(fr => fr.SenderID)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendshipRequest>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverID)
                .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<Comment>()
               .HasOne(c => c.User)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.UserID)
               .OnDelete(DeleteBehavior.NoAction);

        }


    }
}
