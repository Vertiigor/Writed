using System.ComponentModel.DataAnnotations;

namespace Writed.Models
{
    public class Community
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
