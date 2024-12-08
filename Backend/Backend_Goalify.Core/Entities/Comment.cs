using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Contents { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikesCount { get; set; }

        // Foreign keys
        public string UserId { get; set; }
        public string? GoalEntryId { get; set; }
        public string? ParentCommentId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [ForeignKey("GoalEntryId")]
        public GoalEntry? GoalEntry { get; set; }
        
        [ForeignKey("ParentCommentId")]
        public Comment? ParentComment { get; set; }
        
        public ICollection<Comment> Replies { get; set; }
        public ICollection<CommentLikes> CommentLikes { get; set; }

        public Comment()
        {
            Replies = new HashSet<Comment>();
            CommentLikes = new HashSet<CommentLikes>();
        }
    }
} 