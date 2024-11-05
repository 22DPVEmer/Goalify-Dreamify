using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class Tag : BaseModel
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public ICollection<GoalEntry> Goals { get; set; }
    }
} 