using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Writed.Data;
using Writed.Models;

namespace Writed.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;

        public EditModel(Writed.Data.ApplicationContext context)
        {
            this.context = context;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || context.Posts == null)
            {
                return NotFound();
            }

            var post =  await context.Posts.FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return NotFound();
            }
            Post = post;
           ViewData["CommunityId"] = new SelectList(context.Communities, "Id", "Id");
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
