using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Grants
{
    public class IndexModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public IndexModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public IList<Grant> Grant { get;set; } = default!;
        public User User { get; set; }

        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserData") != null)
            {
                User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));

                if (User != null)
                {
                    if (User.IsAdmin)
                    {
                        Grant = await _context.Grant.ToListAsync();
                    }
                    else
                    {
                        //// Grants created by user:

                        //var principalUser = User.FirstName + " " + User.LastName;
                        //Grant = await _context.Grant.Where(g => principalUser == g.PrincipalInvestigator).ToListAsync();

                        /// Grants where user is PI:
                        var userId = User.Id;
                        Grant = await _context.Grant.Where(g => userId == g.GrantUserID).ToListAsync();
                    } 
                }
            }
            
        }
    }
}

