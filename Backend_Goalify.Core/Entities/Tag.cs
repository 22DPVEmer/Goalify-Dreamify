using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class Tag
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string TagName { get; set; }
        
        public string UserId { get; set; }
        public string GoalEntryId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        
        [ForeignKey("GoalEntryId")]
        public GoalEntry GoalEntry { get; set; }
    }
} 