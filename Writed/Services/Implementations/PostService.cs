using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Services.Implementations
{
    public class PostService : IPostService
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly ICommentService commentService;

        public PostService(ApplicationContext context, ICommentService commentService)
        {
            this.context = context;
            this.commentService = commentService;
        }

        public async Task CreatePostAsync(string title, string content, string communityName, User user)
        {
            var community = context.Communities.FirstOrDefault(community => community.Name == communityName);

            if(community == null)
            {
                return;
            }

            Post newPost = new Post()
            {
                Id = Guid.NewGuid().ToString(),
                Author = user,
                Title = title,
                Content = content,
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                Community = community
            };

            context.Posts.Add(newPost);
            await context.SaveChangesAsync();
        }

        public async Task<Post> GetPostAsync(string id)
        {
            var post = await context.Posts.FirstOrDefaultAsync(post => post.Id == id);

            return post;
        }

        public async Task<List<Post>> GetPostsAsync(Community community)
        {
            var posts = await context.Posts.Include(post => post.Author).Where(post => post.Community.Id == community.Id).ToListAsync();

            return posts;
        }

        public async Task UpdatePostAsync(Post post)
        {
            var existingPost = await context.Posts.FindAsync(post.Id);

            if (existingPost != null)
            {
                existingPost.Title = post.Title;
                existingPost.Content = post.Content;

                context.Attach(existingPost);

                context.Entry(existingPost).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }

        public bool PostExists(string id)
        {
            return (context.Posts?.Any(post => post.Id == id)).GetValueOrDefault();
        }

        public async Task DeletePostAsync(Post post)
        {
            context.Posts.Remove(post);

            await commentService.DeleteCommentsAsync(post);

            await context.SaveChangesAsync();
        }

        public async Task DeletePostsAsync(Community community)
        {
            var posts = await GetPostsAsync(community);

            foreach(var post in posts)
            {
                await DeletePostAsync(post);
            }
        }
    }
}
