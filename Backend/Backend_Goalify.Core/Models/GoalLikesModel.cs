

namespace Backend_Goalify.Core.Models
{
    public class GoalLikesModel : BaseModel
    {
        public string UserId { get; set; }
        public string GoalEntryId { get; set; }
        public UserModel User { get; set; }
        public GoalEntryModel GoalEntry { get; set; }
    }
} 