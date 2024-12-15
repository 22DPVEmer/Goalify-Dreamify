using System;
using System.Collections.Generic;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class CommentLikesModel : BaseModel
    {
        public string UserId { get; set; }
        public string CommentId { get; set; }
        public UserModel User { get; set; }
        public CommentModel Comment { get; set; }
    }
} 