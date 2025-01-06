using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;

namespace CS4760_Project.Pages.RubricCriterion
{
    public class DeleteModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DeleteModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RubricCriteria RubricCriteria { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubriccriteria = await _context.RubricCriteria.FirstOrDefaultAsync(m => m.RubricCriteriaId == id);

            if (rubriccriteria == null)
            {
                return NotFound();
            }
            else
            {
                RubricCriteria = rubriccriteria;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubriccriteria = await _context.RubricCriteria.FindAsync(id);
            if (rubriccriteria != null)
            {
                RubricCriteria = rubriccriteria;
                _context.RubricCriteria.Remove(RubricCriteria);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
