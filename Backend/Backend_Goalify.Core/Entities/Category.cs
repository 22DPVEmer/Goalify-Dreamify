using System.ComponentModel.DataAnnotations;

namespace Backend_Goalify.Core.Entities
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation property
        public ICollection<GoalEntry> Goals { get; set; } = new HashSet<GoalEntry>();
    }
}
