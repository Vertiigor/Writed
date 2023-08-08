using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Posts
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly IAuthorizationService authService;
        private readonly IPostService postService;

        public EditModel(Writed.Data.ApplicationContext context, IAuthorizationService authService, IPostService postService)
        {
            this.context = context;
            this.authService = authService;
            this.postService = postService;
            Input = new InputModel();
        }

        public Post Post { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Description")]
            public string Content { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || context.Posts == null)
            {
                return NotFound();
            }

            var post = await postService.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            var authResult = await authService.AuthorizeAsync(User, post, "CanEdit");

            if (!authResult.Succeeded)
            {
                return new ForbidResult();
            }

            Post = post;

            Input.Title = Post.Title;
            Input.Content = Post.Content;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await postService.UpdatePostAsync(Input.Title, Input.Content, id);

            return RedirectToPage("/Index");
        }
    }
}
