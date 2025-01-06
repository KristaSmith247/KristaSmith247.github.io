using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.FinancialRequests
{
    public class EditFinancialRequestModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public EditFinancialRequestModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public Grant Grant { get; set; }
        public string Source { get; set; }

        [BindProperty]
        public FinancialRequest FinancialRequest { get; set; }

        public List<FinancialRequest> FinancialRequests = new List<FinancialRequest>();

        public async Task<IActionResult> OnGetAsync(int id, string source)
        {
            Grant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();
            
            Source = source;

            switch (source)
            {
                case "Equipment":
                    if (Grant.EquipmentRequestIds != null && Grant.EquipmentRequestIds.Any())
                    {
                        FinancialRequests = await _context.FinancialRequest
                            .Where(fr => Grant.EquipmentRequestIds.Contains(fr.RequestId))
                            .ToListAsync();
                    }
                    break;

                case "Travel":
                    if (Grant.TravelRequestIds != null && Grant.TravelRequestIds.Any())
                    {
                        FinancialRequests = await _context.FinancialRequest
                            .Where(fr => Grant.TravelRequestIds.Contains(fr.RequestId))
                            .ToListAsync();
                    }
                    break;

                case "Other":
                    if (Grant.OtherRequestIds != null && Grant.OtherRequestIds.Any())
                    {
                        FinancialRequests = await _context.FinancialRequest
                            .Where(fr => Grant.OtherRequestIds.Contains(fr.RequestId))
                            .ToListAsync();
                    }
                    break;
                default:
                    break;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditFinancialRequest(int RequestId, string Resource1, string Resource2, string RequestTitle, decimal Resource1Amount, decimal Resource2Amount, decimal GrantAmount, decimal Total)
        {


            FinancialRequest existingRequest = _context.FinancialRequest.Where(r => r.RequestId == RequestId).FirstOrDefault();

            Grant = await _context.Grant.Where(g => g.GrantID == existingRequest.GrantID).FirstOrDefaultAsync();

            if (existingRequest == null)
            {
                return NotFound(); // Handle case where request is not found
            }

            existingRequest.Resource1 = Resource1;
            existingRequest.Resource2 = Resource2;
            existingRequest.RequestTitle = RequestTitle;
            existingRequest.Resource1Amount = Resource1Amount;
            existingRequest.Resource2Amount = Resource2Amount;
            existingRequest.GrantAmount = GrantAmount;
            existingRequest.Total = Total;

            await _context.SaveChangesAsync();

            

            return RedirectToPage("/FinancialRequests/Equipment");
        }
    }
}
