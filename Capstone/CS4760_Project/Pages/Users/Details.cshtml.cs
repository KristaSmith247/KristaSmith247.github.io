using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DetailsModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public User User { get; set; } = default!;
        public User AuthenticatedUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (HttpContext.Session.GetString("UserData") != null)
            {
                AuthenticatedUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));

                if (AuthenticatedUser.IsAdmin)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        User = user;
                    }
                    return Page();
                }
            }
            return NotFound();      
        }
    }
}
