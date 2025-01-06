using CS4760_Project.Data;
using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Users
{
    public class LoginModel : PageModel
    {
        private readonly CS4760_ProjectContext _context;

        public LoginModel(CS4760_ProjectContext context)
        {
            _context = context;
        }
        [BindProperty]
        public User User { get; set; } = new User();
        public IList<Models.Committee> Committee { get; set; } = default!;

        /// <summary>
        ///     Handles the GET request for the login page.
        ///     This method is invoked when the user navigates to the login page.
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        ///     Handles the POST request for the login page.
        ///     This method is invoked when the login form is submitted.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            var authenticatedUser = await Authenticate(User.Email, User.Password);

            if (authenticatedUser != null)
            {
                // Clearing the password before storing user data in session
                authenticatedUser.Password = "";

                // Store user data in session
                HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(authenticatedUser));
                return RedirectToPage("/Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }

        /// <summary>
        ///     Authenticates User by verifying Email and Password.
        /// </summary>
        private async Task<User> Authenticate(string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    if (user.IsAdmin)
                    {
                        HttpContext.Session.SetString("Layout", "admin");
                    }
                    else
                    {
                        Committee = await _context.Committee.ToListAsync();
                        foreach (var member in Committee)
                        {
                            if (member.UserId == user.Id)
                            {
                                HttpContext.Session.SetString("Layout", "reviewer");
                                return user;
                            }
                        }

                        HttpContext.Session.SetString("Layout", "");
                    }

                    return user;
                }
            }

            return null;
        }
    }
}
