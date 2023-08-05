using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Writed.Data;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Posts
{
    public class PostModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly ICommentService commentService;

        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }

        public PostModel(Writed.Data.ApplicationContext context, UserManager<User> userManager, ICommentService commentService)
        {
            this.context = context;
            this.userManager = userManager;
            this.commentService = commentService;
            Post = new Post();
            Comments = new List<Comment>();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Content")]
            public string CommentContet { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || context.Posts == null)
            {
                return NotFound();
            }

            var post = await context.Posts.FirstOrDefaultAsync(post => post.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
            }

            Comments = context.Comments.Include(comment => comment.User).Where(comment => comment.Post.Id == Post.Id).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string postId)
        {
            if (!ModelState.IsValid || context.Posts == null)
            {
                return Page();
            }

            // Получение текущего пользователя
            var user = await userManager.GetUserAsync(User);

            await commentService.CreateCommentAsync(Input.CommentContet, postId, user);

            return RedirectToPage("/Index");
        }
    }
}
