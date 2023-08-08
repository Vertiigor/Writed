using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface ICommunityService
    {
        public Task CreateCommunityAsync(string name, string description, User user);

        public Task<Community> GetCommunityAsync(string communityName);

        public Task UpdateCommunityAsync(Community community);
    }
}
