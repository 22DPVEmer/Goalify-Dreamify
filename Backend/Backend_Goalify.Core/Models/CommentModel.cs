using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class CommentModel : BaseModel
    {
        public string Contents { get; set; }  // Changed from Content
        public string UserId { get; set; }
        public string? GoalEntryId { get; set; }
        public string? ParentCommentId { get; set; }
        public int LikesCount { get; set; }
        public UserModel User { get; set; }
        public GoalEntryModel? GoalEntry { get; set; }
        public CommentModel? ParentComment { get; set; }
        public ICollection<CommentModel> Replies { get; set; }
        public virtual ICollection<CommentLikesModel> CommentLikes { get; set; }  // Changed from Likes
    }
}