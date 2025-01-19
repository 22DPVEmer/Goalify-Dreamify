using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend_Goalify.Core.Entities.Enums;

namespace Backend_Goalify.Core.Entities
{
    public class GoalEntry
    {
        [Key]
        public string? Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Title { get; set; } = string.Empty; // Initialize with empty string
        
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsPublic { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public GoalStatus? Status { get; set; }
        public GoalPriority? Priority { get; set; }
        public bool IsActive { get; set; }

        // Foreign key
        public string? UserId { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
        public ICollection<GoalLikes>? Likes { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();
        public virtual ICollection<Category> Categories { get; set; } = new HashSet<Category>();

        public GoalEntry()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsActive = true;
            Priority = GoalPriority.Medium;
            Likes = new HashSet<GoalLikes>();
            Comments = new HashSet<Comment>();
            Tags = new HashSet<Tag>();
            Categories = new HashSet<Category>();
        }
    }
}