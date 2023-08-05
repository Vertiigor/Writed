using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface ICommunityService
    {
        public Task CreateCommunityAsync(string name, string description, User user);
    }
}
