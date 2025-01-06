using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Grants
{
    public class DeleteModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DeleteModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Grant Grant { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grant = await _context.Grant.FirstOrDefaultAsync(m => m.GrantID == id);

            if (grant == null)
            {
                return NotFound();
            }
            else
            {
                Grant = grant;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grant = await _context.Grant.FindAsync(id);
            if (grant != null)
            {
                Grant = grant;
                _context.Grant.Remove(Grant);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
