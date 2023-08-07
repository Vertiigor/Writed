using System.ComponentModel.DataAnnotations;
using Writed.Models.Interfaces;

namespace Writed.Models
{
    public class Comment : IHasAuthor
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
