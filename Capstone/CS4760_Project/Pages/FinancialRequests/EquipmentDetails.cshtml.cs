using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CS4760_Project.Pages.FinancialRequests
{
    public class EquipmentDetailsModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public EquipmentDetailsModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public Grant Grant { get; set; }
        public string Source { get; set; }

        public List<FinancialRequest> FinancialRequests = new List<FinancialRequest>();

        public async Task<IActionResult> OnGetAsync(int id, string source)
        {
            Source = source;

            Grant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();

            FinancialRequests = await _context.FinancialRequest
                .Where(fr => Grant.EquipmentRequestIds.Contains(fr.RequestId))
                .ToListAsync();

            return Page();
        }
    }
}
