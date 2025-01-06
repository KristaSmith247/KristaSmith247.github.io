using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Colleges
{
    public class DeleteModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DeleteModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public College College { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var college = await _context.College.FirstOrDefaultAsync(m => m.CollegeId == id);

            if (college == null)
            {
                return NotFound();
            }
            else
            {
                College = college;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var college = await _context.College.FindAsync(id);
            if (college != null)
            {
                College = college;
                _context.College.Remove(College);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
