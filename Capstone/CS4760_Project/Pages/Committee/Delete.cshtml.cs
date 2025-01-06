using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Committee
{
    public class DeleteModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DeleteModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Committee Committee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var committee = await _context.Committee.FirstOrDefaultAsync(m => m.Id == id);

            if (committee == null)
            {
                return NotFound();
            }
            else
            {
                Committee = committee;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var committee = await _context.Committee.FindAsync(id);
            if (committee != null)
            {
                Committee = committee;
                _context.Committee.Remove(Committee);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
