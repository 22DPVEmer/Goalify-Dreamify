using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Goalify.Core.Entities
{
    public class Attachment
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }
        
        [Required]
        public string MessageId { get; set; }

        [ForeignKey(nameof(MessageId))]
        public Message Message { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        public string FileName { get; set; }
    }
} 