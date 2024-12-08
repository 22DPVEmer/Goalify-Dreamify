using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class Comment : BaseModel
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public string GoalEntryId { get; set; }
        public UserModel User { get; set; }
        public GoalEntry GoalEntry { get; set; }
        public virtual ICollection<CommentLikes> Likes { get; set; }
    }
} 