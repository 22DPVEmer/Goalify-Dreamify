using Backend_Goalify.Core.Models.Enums;


namespace Backend_Goalify.Core.Models
{
public class GoalEntry : BaseModel
{
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public GoalStatus Status { get; set; }
        public GoalPriority Priority { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
        public  ICollection<Tag> Tags { get; set; }
        public  ICollection<Comment> Comments { get; set; }
        public ICollection<GoalLikes> Likes { get; set; }
}
} 