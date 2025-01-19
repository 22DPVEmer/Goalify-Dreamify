using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class Message
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime SentAt { get; set; }

        public string SenderId { get; set; }
        public string ChatId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(ChatId))]
        public Chat Chat { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
    }
} 