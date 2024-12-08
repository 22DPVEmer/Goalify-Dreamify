using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class CommentLikes : BaseModel
    {
        public string UserId { get; set; }
        public string CommentId { get; set; }
        public UserModel User { get; set; }
        public Comment Comment { get; set; }
    }
} 