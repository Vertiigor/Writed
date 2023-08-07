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

        public PostService(ApplicationContext context)
        {
            this.context = context;
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
    }
}
