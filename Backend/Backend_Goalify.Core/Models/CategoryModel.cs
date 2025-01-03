namespace Backend_Goalify.Core.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
} 