namespace Backend_Goalify.Core.Models
{
    public abstract class BaseModel
    {
        public string Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
} 