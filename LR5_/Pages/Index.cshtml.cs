using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace LR5_.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Value = string.Empty;
            ExpirationDateString = string.Empty;
        }

        [BindProperty]
        public string Value { get; set; }

        [BindProperty]
        public string ExpirationDateString { get; set; }

        [BindProperty]
        public DateTime? ExpirationDate { get; set; }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Value) || string.IsNullOrWhiteSpace(ExpirationDateString))
            {
                TempData["ErrorMessage"] = "���� �����, �������� ������ ����.";
                return Page();
            }

            if (!DateTime.TryParseExact(ExpirationDateString, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime expirationDate))
            {
                TempData["ErrorMessage"] = "������������ ������ ����.";
                return Page();
            }

            if (expirationDate < DateTime.Now)
            {
                TempData["ErrorMessage"] = "���� �� ���� ���� � ��������.";
                return Page();
            }

            ExpirationDate = expirationDate;

            if (ExpirationDate.HasValue)
            {
                Response.Cookies.Append("MyCookie", Value, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = ExpirationDate.Value,
                    HttpOnly = true 
                });

                Console.WriteLine($"Cookie Value: {Value}");
            }
            else
            {
                TempData["ErrorMessage"] = "������� ��� ����������� ���� �� ����";
                return Page();
            }

            return RedirectToPage("/CheckCookie");
        }
    }
}