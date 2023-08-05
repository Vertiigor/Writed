using System.ComponentModel.DataAnnotations;

namespace Writed.Models
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Post Post { get; set; }
    }
}
