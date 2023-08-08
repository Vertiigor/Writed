using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Communities
{
    public class EditModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly ICommunityService communityService;
        private readonly IAuthorizationService authService;

        public EditModel(Writed.Data.ApplicationContext context, IAuthorizationService authService, ICommunityService communityService)
        {
            this.context = context;
            this.authService = authService;
            this.communityService = communityService;
            Input = new InputModel();
        }

        public Community Community { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || context.Communities == null)
            {
                return NotFound();
            }

            var community = await context.Communities.FirstOrDefaultAsync(community => community.Id == id);

            if (community == null)
            {
                return NotFound();
            }

            var authResult = await authService.AuthorizeAsync(User, community, "CanManage");

            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            Community = community;

            Input.Name = Community.Name;
            Input.Description = Community.Description;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string communityName)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var community = await communityService.GetCommunityAsync(communityName);

            community.Name = Input.Name;
            community.Description = Input.Description;

            await communityService.UpdateCommunityAsync(community);

            return RedirectToPage("/Index");
        }

        private bool CommunityExists(string id)
        {
            return (context.Communities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
