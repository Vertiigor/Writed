using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface IPostService
    {
        public Task CreatePostAsync(string title, string content, string communityName, User user);
    }
}
