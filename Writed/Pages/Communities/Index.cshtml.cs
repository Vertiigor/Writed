using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Writed.Pages.Communities
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Search")]
            public string SearchString { get; set; }
        }

        public IActionResult OnPostSearch()
        {
            return RedirectToPage("Community", new { communityName = Input.SearchString });
        }

        public IActionResult OnPostCreateNewCommunity()
        {
            return RedirectToPage("Create");
        }
    }
}
