using Microsoft.AspNetCore.Identity;
using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Services.Implementations
{
    public class CommunityService : ICommunityService
    {
        private readonly Writed.Data.ApplicationContext context;

        public CommunityService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task CreateCommunityAsync(string name, string description, User currentUser)
        {
            Community community = new Community()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                CreatedDate = DateTime.UtcNow.ToUniversalTime(),
                AuthorId = currentUser.Id,
                Description = description,
                Posts = new List<Post>()
            };

            context.Communities.Add(community);
            await context.SaveChangesAsync();
        }
    }
}
