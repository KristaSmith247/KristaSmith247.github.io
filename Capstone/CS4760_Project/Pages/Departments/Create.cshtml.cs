using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS4760_Project.Data;
using CS4760_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace CS4760_Project.Pages.Departments
{
    public class CreateModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public CreateModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        // public SelectList Colleges { get; set; }

        public IList<College> Colleges { get; set; } = default!;
     
        public async Task<IActionResult> OnGet()
        {
            // Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Colleges = await _context.College.ToListAsync();
            


            return Page();
        }

        [BindProperty]
        public Department Department { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           // Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Colleges = await _context.College.ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Department.Add(Department);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
