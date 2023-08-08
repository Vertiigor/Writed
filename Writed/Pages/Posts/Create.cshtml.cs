using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Writed.Models;
using Writed.Services.Interfaces;

namespace Writed.Pages.Posts
{
    [Authorize("IsAnAdult")]
    public class CreateModel : PageModel
    {
        private readonly Writed.Data.ApplicationContext context;
        private readonly UserManager<User> userManager;
        private readonly IPostService postService;
        public string CommunityName;

        public CreateModel(Writed.Data.ApplicationContext context, UserManager<User> userManager, IPostService postService)
        {
            this.context = context;
            this.userManager = userManager;
            this.postService = postService;
            CommunityName = string.Empty;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(string communityName)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // Если пользователь не авторизован, перенаправляем его на страницу входа
                return RedirectToPage("~/Identity/Pages/Account/Login");
            }

            this.CommunityName = communityName;

            Console.WriteLine(communityName);

            return Page();
        }

        public class InputModel
        {
            [Required]
            [Display(Name = "Title")]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Content")]
            public string Content { get; set; }
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(string communityName)
        {
            if (!ModelState.IsValid || context.Posts == null)
            {
                return Page();
            }

            // Получение текущего пользователя
            var user = await userManager.GetUserAsync(User);

            await postService.CreatePostAsync(Input.Title, Input.Content, communityName, user);

            return RedirectToPage("/Index");
        }
    }
}
