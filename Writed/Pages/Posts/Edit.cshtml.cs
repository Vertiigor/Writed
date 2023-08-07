using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

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

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            context.Attach(Post).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PostExists(string id)
        {
          return (context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
