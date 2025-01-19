using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class GoalLikes
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        public DateTime CreatedAt { get; set; }

        // Foreign keys
        public string UserId { get; set; }
        public string GoalEntryId { get; set; }

        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [ForeignKey("GoalEntryId")]
        public GoalEntry GoalEntry { get; set; }
    }
} 