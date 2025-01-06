using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CS4760_Project.Pages.FinancialRequests
{
    public class TravelDetailsModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public TravelDetailsModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public Grant Grant { get; set; }
        public string Source { get; set; }

        public List<FinancialRequest> FinancialRequests;

        public async Task<IActionResult> OnGetAsync(int id, string source)
        {
            Source = source;

            Grant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();

            FinancialRequests = await _context.FinancialRequest
                .Where(fr => Grant.TravelRequestIds.Contains(fr.RequestId))
                .ToListAsync();

            return Page();
        }
    }
}
