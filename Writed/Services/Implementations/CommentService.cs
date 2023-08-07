using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private readonly Writed.Data.ApplicationContext context;

        public CommentService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task CreateCommentAsync(string content, string postId, User user)
        {
            var post = context.Posts.FirstOrDefault(post => post.Id == postId);

            if (post == null)
            {
                return;
            }

            Comment newComment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Content = content,
                Post = post,
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                Author = user
            };
            
            context.Comments.Add(newComment);
            await context.SaveChangesAsync();
        }
    }
}
