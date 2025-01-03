using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class TagModel : BaseModel
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        public string GoalEntryId { get; set; }
        public UserModel User { get; set; }

        public GoalEntryModel GoalEntry { get; set; }
        //public ICollection<GoalEntryModel> Goals { get; set; }
    }
} 