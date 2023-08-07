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

namespace Writed.Pages.Communities
{
    [Authorize]
    public class CommunityModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        public List<Post> Posts { get; set; }
        public Community Community { get; set; } 

        public CommunityModel(Writed.Data.ApplicationContext context)
        {
            this.context = context;
            Posts = new List<Post>();
            Community = new Community();
        }


        public async Task<IActionResult> OnGetAsync(string communityName)
        {
            if (communityName == null || context.Communities == null)
            {
                return NotFound();
            }

            var community = await context.Communities.FirstOrDefaultAsync(community => community.Name == communityName);

            if (community == null)
            {
                return NotFound();
            }
            else 
            {
                Community = community;
            }

            Posts = context.Posts.Include(post => post.Author).Where(post => post.Community.Id == community.Id).ToList();

            return Page();
        }
    }
}
