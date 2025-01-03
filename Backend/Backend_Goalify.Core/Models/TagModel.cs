using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class TagModel : BaseModel
    {
        public string Name { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int UsageCount { get; set; }

        public UserModel? User { get; set; }
        public ICollection<GoalEntryModel> Goals { get; set; } = new List<GoalEntryModel>();
    }
} 