﻿using Writed.Models;

namespace Writed.Services.Interfaces
{
    public interface IPostService
    {
        public Task CreatePostAsync(string title, string content, string communityName, User user);

        public Task<Post> GetPostAsync(string id);

        public Task<List<Post>> GetPostsAsync(Community community);

        public Task UpdatePostAsync(Post post);

        public Task DeletePostAsync(Post post);

        public Task DeletePostsAsync(Community community);

        public bool PostExists(string id);
    }
}
