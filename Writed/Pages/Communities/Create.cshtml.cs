using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Communities
{
    [Authorize("IsAnAdult")]
    public class CreateModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly ICommunityService communityService;

        public CreateModel(Writed.Data.ApplicationContext context, UserManager<User> userManager, ICommunityService communityService)
        {
            this.context = context;
            this.userManager = userManager;
            this.communityService = communityService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Description")]
            public string Description { get; set; }
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || context.Communities == null)
            {
                return Page();
            }

            // Getting current user
            var user = await userManager.GetUserAsync(User);

            // Creating new community
            await communityService.CreateCommunityAsync(Input.Name, Input.Description, user);

            return RedirectToPage("/Index");
        }
    }
}
