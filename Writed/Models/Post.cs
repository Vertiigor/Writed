using System.ComponentModel.DataAnnotations;

namespace Writed.Models
{
    public class Post
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Community Community { get; set; }

        [Required]
        public string CommunityId { get; set; }
    }
}
