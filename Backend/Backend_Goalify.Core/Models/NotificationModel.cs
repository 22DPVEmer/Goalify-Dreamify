using Backend_Goalify.Core.Models.Enums;


namespace Backend_Goalify.Core.Models
{
    public class NotificationModel : BaseModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public UserModel User { get; set; }
    }
} 