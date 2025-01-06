using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Committee
{
    public class CreateModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public CreateModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Committee Committee { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Committee.Add(Committee);
            await _context.SaveChangesAsync();

            return RedirectToPage("./RSPG_Reviewers");
        }
    }
}
