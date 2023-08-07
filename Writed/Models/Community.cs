using System.ComponentModel.DataAnnotations;
using Writed.Models.Interfaces;

namespace Writed.Models
{
    public class Community : IHasAuthor
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}
