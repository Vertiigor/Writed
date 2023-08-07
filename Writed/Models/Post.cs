using System.ComponentModel.DataAnnotations;
using Writed.Models.Interfaces;

namespace Writed.Models
{
    public class Post : IHasAuthor
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public Community Community { get; set; }

        [Required]
        public string CommunityId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
