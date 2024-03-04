using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LR5_.Pages
{
    public class CheckCookieModel : PageModel
    {
        public CheckCookieModel()
        {
            CookieValue = string.Empty;
        }

        public string CookieValue { get; set; }

        public void OnGet()
        {
            CookieValue = Request.Cookies["MyCookie"];
        }
    }
}
