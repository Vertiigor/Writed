using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Communities
{
    [Authorize]
    public class CommunityModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly ICommunityService communityService;
        private readonly IPostService postService;
        public List<Post> Posts { get; set; }
        public Community Community { get; set; } 

        public CommunityModel(Writed.Data.ApplicationContext context, IPostService postService, ICommunityService communityService)
        {
            this.context = context;
            this.postService = postService;
            Posts = new List<Post>();
            Community = new Community();
            this.communityService = communityService;
        }


        public async Task<IActionResult> OnGetAsync(string communityName)
        {
            if (communityName == null || context.Communities == null)
            {
                return NotFound();
            }

            var community = await communityService.GetCommunityAsync(communityName);

            if (community == null)
            {
                return NotFound();
            }
            else 
            {
                Community = community;
            }

            Posts = await postService.GetPostsAsync(community);

            return Page();
        }
    }
}
