using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class CommentLikes
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public DateTime CreatedAt { get; set; }
        public bool IsLiked { get; set; }

        // Foreign keys
        public string UserId { get; set; }
        public string CommentId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [ForeignKey("CommentId")]
        public Comment Comment { get; set; }
    }
} 