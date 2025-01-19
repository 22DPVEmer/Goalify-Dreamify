using Microsoft.AspNetCore.Identity;

namespace Backend_Goalify.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? LastLogin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiry { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        // Navigation properties
        public ICollection<GoalEntry> GoalEntries { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<GoalLikes> GoalLikes { get; set; }
        public ICollection<CommentLikes> CommentLikes { get; set; }
        public ICollection<Moderator> Moderators { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        
        public ApplicationUser()
        {
            GoalEntries = new HashSet<GoalEntry>();
            Tags = new HashSet<Tag>();
            Comments = new HashSet<Comment>();
            GoalLikes = new HashSet<GoalLikes>();
            CommentLikes = new HashSet<CommentLikes>();
            Moderators = new HashSet<Moderator>();
            Notifications = new HashSet<Notification>();
            UserRoles = new HashSet<IdentityUserRole<string>>();
        }
    }
}