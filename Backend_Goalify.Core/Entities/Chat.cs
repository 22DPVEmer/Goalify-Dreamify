using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class Chat
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string User1Id { get; set; }
        public string User2Id { get; set; }

        [ForeignKey("User1Id")]
        public ApplicationUser User1 { get; set; }
        
        [ForeignKey("User2Id")]
        public ApplicationUser User2 { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
} 