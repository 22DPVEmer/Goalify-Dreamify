using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend_Goalify.Core.Entities
{
    public class Tag
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        public int UsageCount { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        // Many-to-many relationship
        public virtual ICollection<GoalEntry> Goals { get; set; }

        public Tag()
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            Goals = new HashSet<GoalEntry>();
        }
    }
}