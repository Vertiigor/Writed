﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Communities
{
    public class CommunityModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly ICommunityService communityService;
        private readonly IAuthorizationService authService;
        private readonly IPostService postService;

        public List<Post> Posts { get; set; }
        public Community Community { get; set; }
        public bool CanManage { get; set; }

        public CommunityModel(Writed.Data.ApplicationContext context, IPostService postService, ICommunityService communityService, IAuthorizationService authService)
        {
            this.context = context;
            this.postService = postService;
            Posts = new List<Post>();
            Community = new Community();
            this.communityService = communityService;
            this.authService = authService;
            CanManage = false;
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

            var authResult = await authService.AuthorizeAsync(User, community, "CanManage");

            CanManage = authResult.Succeeded;

            Posts = await postService.GetPostsAsync(community);

            return Page();
        }

        public async Task<IActionResult> OnPostDelete(string communityName)
        {
            var community = await communityService.GetCommunityAsync(communityName);

            await communityService.DeleteCommunityAsync(community);

            return RedirectToPage("/Index");
        }
    }
}
