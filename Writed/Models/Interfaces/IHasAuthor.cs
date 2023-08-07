using System.ComponentModel.DataAnnotations;

namespace Writed.Models.Interfaces
{
    public interface IHasAuthor
    {
        [Required]
        public User Author { get; set; }
    }
}
