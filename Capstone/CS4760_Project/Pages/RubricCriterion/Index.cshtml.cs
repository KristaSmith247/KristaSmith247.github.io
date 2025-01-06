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
    public class IndexModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public IndexModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public IList<RubricCriteria> RubricCriteria { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RubricCriteria = await _context.RubricCriteria.ToListAsync();
        }
    }
}
