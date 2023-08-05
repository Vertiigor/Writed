using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Writed.Data;
using Writed.Models;
using Writed.Services.Implementations;
using Writed.Services.Interfaces;

namespace Writed.Pages.Communities
{
    [Authorize("IsAnAdult")]
    public class CreateModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICommunityService _communityService;

        public CreateModel(Writed.Data.ApplicationContext context, UserManager<User> userManager, ICommunityService communityService)
        {
            _context = context;
            _userManager = userManager;
            _communityService = communityService;
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
          if (!ModelState.IsValid || _context.Communities == null)
          {
                return Page();
          }

            // Getting current user
            var user = await _userManager.GetUserAsync(User);

            // Creating new community
            await _communityService.CreateCommunityAsync(Input.Name, Input.Description, user);

            return RedirectToPage("/Index");
        }
    }
}
