using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface ICommentService
    {
        public Task CreateCommentAsync(string content, string postId, User user);

        public Task<Comment> GetCommentAsync(string id);

        public Task<List<Comment>> GetCommentsAsync(Post post);

        public Task DeleteCommentsAsync(Post post);
    }
}
