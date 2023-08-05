using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface ICommentService
    {
        public Task CreateCommentAsync(string content, string postId, User user);
    }
}
