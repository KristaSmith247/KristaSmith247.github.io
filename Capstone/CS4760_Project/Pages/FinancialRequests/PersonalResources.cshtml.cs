using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.FinancialRequests
{
    public class PersonalResourcesModel : PageModel
    {
        [BindProperty]
        public FinancialRequest FinancialRequest { get; set; }
        public List<FinancialRequest> FinancialRequests { get; set; }
        public int TempId { get; set; }

        public void OnGet()
        {
            string sessionData = HttpContext.Session.GetString("PersonalResources");

            if (sessionData == null)
            {
                FinancialRequests = new List<FinancialRequest>();
            }
            else
            {
                FinancialRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(sessionData);
            }
        }

        public IActionResult OnPost()
        {
            string sessionData = HttpContext.Session.GetString("PersonalResources");

            if (sessionData == null)
            {
                FinancialRequests = new List<FinancialRequest>();
            }
            else
            {
                FinancialRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(sessionData);
            }

            string tempId = HttpContext.Session.GetString("TempId");

            if (tempId == null)
            {
                TempId = 0;
            }
            else
            {
                TempId = JsonConvert.DeserializeObject<int>(tempId);
            }

            TempId++;

            // TODO: Set FinancialRequest.Tax here?
            FinancialRequest.TemporaryId = TempId;
            FinancialRequest.RequestType = "PersonalResources";
            FinancialRequest.GrantType = "RSPG";

            FinancialRequests.Add(FinancialRequest);

            // Save back to session
            HttpContext.Session.SetString("PersonalResources", JsonConvert.SerializeObject(FinancialRequests));
            HttpContext.Session.SetString("TempId", JsonConvert.SerializeObject(TempId));

            return RedirectToPage("/FinancialRequests/PersonalResources");
        }

        // This method will handle the form post for editing the financial request
        public async Task<IActionResult> OnPostEditFinancialRequest(bool IsStudent, int RequestId, string Resource1, string Resource2, string RequestTitle, decimal Resource1Amount, decimal Resource2Amount, decimal GrantAmount, decimal Total, decimal TotalWithTaxes)
        {
            string sessionData = HttpContext.Session.GetString("PersonalResources");

            if (sessionData == null)
            {
                FinancialRequests = new List<FinancialRequest>();
            }
            else
            {
                FinancialRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(sessionData);
            }

            FinancialRequest existingRequest = FinancialRequests.Where(r => r.TemporaryId == RequestId).FirstOrDefault();

            if (existingRequest == null)
            {
                return NotFound(); // Handle case where request is not found
            }

            existingRequest.IsStudent = IsStudent;
            existingRequest.Resource1 = Resource1;
            existingRequest.Resource2 = Resource2;
            existingRequest.RequestTitle = RequestTitle;
            existingRequest.Resource1Amount = Resource1Amount;
            existingRequest.Resource2Amount = Resource2Amount;
            existingRequest.GrantAmount = GrantAmount;
            existingRequest.Total = Total;
            existingRequest.TotalWithTaxes = TotalWithTaxes;

            // Save back to session
            HttpContext.Session.SetString("PersonalResources", JsonConvert.SerializeObject(FinancialRequests));

            return RedirectToPage("/FinancialRequests/PersonalResources");
        }

        // This method will handle the form post for editing the financial request
        public async Task<IActionResult> OnPostDeleteFinancialRequest(int RequestId)
        {
            string sessionData = HttpContext.Session.GetString("PersonalResources");

            if (sessionData == null)
            {
                FinancialRequests = new List<FinancialRequest>();
            }
            else
            {
                FinancialRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(sessionData);
            }

            FinancialRequest existingRequest = FinancialRequests.Where(r => r.TemporaryId == RequestId).FirstOrDefault();

            if (existingRequest != null)
            {
                FinancialRequests.Remove(existingRequest);
            }

            // Save back to session
            HttpContext.Session.SetString("PersonalResources", JsonConvert.SerializeObject(FinancialRequests));

            return RedirectToPage("/FinancialRequests/PersonalResources");
        }
    }
}
