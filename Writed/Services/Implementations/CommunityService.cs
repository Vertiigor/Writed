using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly IPostService postService;

        public CommunityService(ApplicationContext context, IPostService postService)
        {
            this.context = context;
            this.postService = postService;
        }

        public async Task CreateCommunityAsync(string name, string description, User user)
        {
            Community community = new Community()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                Author = user,
                Description = description,
                Posts = new List<Post>()
            };

            context.Communities.Add(community);
            await context.SaveChangesAsync();
        }

        public async Task DeleteCommunityAsync(Community community)
        {
            await postService.DeletePostsAsync(community);

            context.Communities.Remove(community);

            await context.SaveChangesAsync();
        }

        public async Task<Community> GetCommunityAsync(string communityName)
        {
            var community = await context.Communities.FirstOrDefaultAsync(community => community.Name == communityName);

            return community;
        }

        public async Task UpdateCommunityAsync(Community community)
        {
            var existingCommunity = await context.Communities.FindAsync(community.Id);

            if (existingCommunity != null)
            {
                existingCommunity.Name = community.Name;
                existingCommunity.Description = community.Description;

                context.Attach(existingCommunity);

                context.Entry(existingCommunity).State = EntityState.Modified;

                await context.SaveChangesAsync();
            }
        }
    }
}
