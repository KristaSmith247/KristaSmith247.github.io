using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.Committee
{
    public class EditModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public EditModel(CS4760_Project.Data.CS4760_ProjectContext context)
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

            var committee =  await _context.Committee.FirstOrDefaultAsync(m => m.Id == id);
            if (committee == null)
            {
                return NotFound();
            }
            Committee = committee;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Committee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommitteeExists(Committee.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CommitteeExists(int id)
        {
            return _context.Committee.Any(e => e.Id == id);
        }
    }
}
