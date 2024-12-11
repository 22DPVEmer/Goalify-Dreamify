using Backend_Goalify.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend_Goalify.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role,string>
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLikes> CommentLikes { get; set; }
        public DbSet<GoalEntry> GoalEntries { get; set; }
        public DbSet<GoalLikes> GoalLikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure enum conversions
            builder.Entity<GoalEntry>()
                .Property(g => g.Status)
                .HasConversion<string>();

            builder.Entity<GoalEntry>()
                .Property(g => g.Priority)
                .HasConversion<string>();

            // GoalEntry relationships
            builder.Entity<GoalEntry>()
                .HasOne(g => g.User)
                .WithMany(u => u.GoalEntries)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // GoalLikes relationships
            builder.Entity<GoalLikes>()
                .HasOne(gl => gl.GoalEntry)
                .WithMany(g => g.Likes)
                .HasForeignKey(gl => gl.GoalEntryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Notification>()
    .HasOne(n => n.User)
    .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<GoalLikes>()
                .HasOne(gl => gl.User)
                .WithMany(u => u.GoalLikes)
            .HasForeignKey(gl => gl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment relationships
            builder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.GoalEntry)
                .WithMany(g => g.Comments)
                .HasForeignKey(c => c.GoalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // CommentLikes relationships
            builder.Entity<CommentLikes>()
                .HasOne(cl => cl.Comment)
                .WithMany(c => c.CommentLikes)
                .HasForeignKey(cl => cl.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CommentLikes>()
                .HasOne(cl => cl.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Tag relationships
            builder.Entity<Tag>()
                .HasOne(t => t.GoalEntry)
                .WithMany(g => g.Tags)
                .HasForeignKey(t => t.GoalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Tag>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tags)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Moderator relationships
            builder.Entity<Moderator>()
                .HasOne(m => m.GoalEntry)
                .WithMany()
                .HasForeignKey(m => m.GoalEntryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Moderator>()
                .HasOne(m => m.User)
                .WithMany(u => u.Moderators)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Chat and Message relationships
            builder.Entity<Chat>()
                .HasOne(c => c.User1)
            .WithMany()
                .HasForeignKey(c => c.User1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Chat>()
                .HasOne(c => c.User2)
                .WithMany()
                .HasForeignKey(c => c.User2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChatId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Attachment relationships
            builder.Entity<Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}