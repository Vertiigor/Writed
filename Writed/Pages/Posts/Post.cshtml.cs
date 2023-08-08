using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Posts
{
    public class PostModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly ICommentService commentService;
        private readonly IAuthorizationService authService;
        private readonly IPostService postService;

        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public bool CanManage { get; set; }

        public PostModel(Writed.Data.ApplicationContext context, UserManager<User> userManager, ICommentService commentService, IAuthorizationService authService, IPostService postService)
        {
            this.context = context;
            this.userManager = userManager;
            this.commentService = commentService;
            this.authService = authService;
            Post = new Post();
            Comments = new List<Comment>();
            CanManage = false;
            this.postService = postService;
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

            var post = await postService.GetPostAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            else
            {
                Post = post;
            }

            var authResult = await authService.AuthorizeAsync(User, post, "CanManage");

            CanManage = authResult.Succeeded;

            Comments = await commentService.GetCommentsAsync(post);

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

        public async Task<IActionResult> OnPostDelete(string postId)
        {
            var post = await postService.GetPostAsync(postId);

            await postService.DeletePostAsync(post);

            return RedirectToPage("/Index");
        }
    }
}
