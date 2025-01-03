using Backend_Goalify.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Backend_Goalify.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GoalEntry> GoalEntries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<GoalLikes> GoalLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Category> Categories { get; set; }

        // Add DbSet for IdentityRole
        public DbSet<IdentityRole> Roles { get; set; }

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
            
            // Configure many-to-many relationship between GoalEntry and Tag
            builder.Entity<GoalEntry>()
                .HasMany(g => g.Tags)
                .WithMany(t => t.Goals)
                .UsingEntity<Dictionary<string, object>>(
                    "GoalEntryTags",
                    j => j.HasOne<Tag>()
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<GoalEntry>()
                        .WithMany()
                        .HasForeignKey("GoalEntryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property<string>("GoalEntryId").IsRequired();
                        j.Property<string>("TagId").IsRequired();
                        j.HasKey("GoalEntryId", "TagId");
                    }
                );

            builder.Entity<GoalEntry>()
                .HasMany(g => g.Categories)
                .WithMany(c => c.Goals)
                .UsingEntity<Dictionary<string, object>>(
                    "GoalEntryCategories",
                    j => j.HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<GoalEntry>()
                        .WithMany()
                        .HasForeignKey("GoalEntryId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property<string>("GoalEntryId").IsRequired();
                        j.Property<string>("CategoryId").IsRequired();
                        j.HasKey("GoalEntryId", "CategoryId");
                    }
                );

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

            // Other relationships
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

            builder.Entity<Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}