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
        public string Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UsageCount { get; set; }


        // Foreign key for GoalEntry
        public string GoalEntryId { get; set; }
        [ForeignKey("GoalEntryId")]
        [JsonIgnore]
        public GoalEntry GoalEntry { get; set; }

        public string UserId { get; set; }
        [ForeignKey("CreatedById")]
        public ApplicationUser User { get; set; }

        public ICollection<GoalEntry> Goals { get; set; }
    }
}