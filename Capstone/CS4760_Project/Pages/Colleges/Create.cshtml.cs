using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Colleges
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
        public College College { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.College.Add(College);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
