using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Models
{
    public class GoalEntryModel : BaseModel
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public GoalStatus? Status { get; set; }
        public GoalPriority? Priority { get; set; }
        public string? UserId { get; set; }
        public ICollection<TagModel>? Tags { get; set; }
        public ICollection<CommentModel>? Comments { get; set; }
        public ICollection<GoalLikesModel>? Likes { get; set; }
        public bool IsPublic { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
    }
}