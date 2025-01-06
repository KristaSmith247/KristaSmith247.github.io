using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Data;
using CS4760_Project.Models;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Grants
{
    public class DetailsModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public DetailsModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public Grant Grant { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public College College { get; set; }
        public Models.Committee Committee { get; set; }
        public List<Review> Reviews { get; set; }
        public List<FinancialRequest> PersonalResourceRequests { get; set; } = new List<FinancialRequest>();
        public List<FinancialRequest> EquipmentRequests { get; set; } = new List<FinancialRequest>();
        public List<FinancialRequest> TravelRequests { get; set; } = new List<FinancialRequest>();
        public List<FinancialRequest> OtherRequests { get; set; } = new List<FinancialRequest>();

        // Financial Request Amounts
        public decimal PersonalResourceRSPGAmount { get; set; }
        public decimal PersonalResourceAvailableAmount { get; set; }
        public decimal PersonalResourceTotalAmount { get; set; }
        public decimal EquipmentRSPGAmount { get; set; }
        public decimal EquipmentAvailableAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public decimal TravelRSPGAmount { get; set; }
        public decimal TravelAvailableAmount { get; set; }
        public decimal TravelTotalAmount { get; set; }
        public decimal OtherRSPGAmount { get; set; }
        public decimal OtherAvailableAmount { get; set; }
        public decimal OtherTotalAmount { get; set; }

        // Total Grant Amounts
        public decimal RSPGAmount { get; set; } = 0;
        public decimal AvailableAmount { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Grant = await _context.Grant.Where(c => c.GrantID == id).FirstOrDefaultAsync();
            Reviews = await _context.Review.Where(c => Grant.ReviewIDs.Contains(c.ReviewId)).ToListAsync();

            College = await _context.College.Where(c => c.CollegeId == Grant.GrantCollegeID).FirstOrDefaultAsync();

            if (College == null)
            {
                College = new College();
            }

            Department = await _context.Department.Where(d => d.DepartmentId == Grant.GrantDepartmentID).FirstOrDefaultAsync();

            if (Department == null)
            {
                Department = new Department();
            }


            if (HttpContext.Session.GetString("UserData") != null)
            {
                User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

            PersonalResourceRequests = await _context.FinancialRequest
                .Where(fr => Grant.PersonalResourceRequestIds
                .Contains(fr.RequestId))
                .ToListAsync();

            EquipmentRequests = await _context.FinancialRequest
                .Where(fr => Grant.EquipmentRequestIds
                .Contains(fr.RequestId))
                .ToListAsync();

            TravelRequests = await _context.FinancialRequest
                .Where(fr => Grant.TravelRequestIds
                .Contains(fr.RequestId))
                .ToListAsync();

            OtherRequests = await _context.FinancialRequest
                .Where(fr => Grant.OtherRequestIds
                .Contains(fr.RequestId))
                .ToListAsync();

            // Create a list of lists for financial requests
            var allRequests = new List<List<FinancialRequest>>
            {
                PersonalResourceRequests,
                EquipmentRequests,
                TravelRequests,
                OtherRequests
            };

            CalculateTotals(allRequests);

            EquipmentTotalAmount = EquipmentRSPGAmount + EquipmentAvailableAmount;
            TravelTotalAmount = TravelRSPGAmount + TravelAvailableAmount;
            OtherTotalAmount = OtherRSPGAmount + OtherAvailableAmount;
            TotalAmount = RSPGAmount + AvailableAmount;

            return Page();
        }

        private void CalculateTotals(List<List<FinancialRequest>> allRequests)
        {
            int index;

            RSPGAmount = 0;
            AvailableAmount = 0;

            foreach (var requests in allRequests)
            {
                index = allRequests.IndexOf(requests);

                foreach (var request in requests)
                {
                    if (request != null)
                    {
                        // Sum up the GrantAmount
                        RSPGAmount += request.GrantAmount; // Handle null GrantAmount as 0

                        // Sum up the available resources
                        AvailableAmount += (request.Resource1Amount + request.Resource2Amount);

                        switch (index)
                        {
                            case 0:
                                PersonalResourceRSPGAmount += request.GrantAmount;
                                PersonalResourceAvailableAmount += (request.Resource1Amount + request.Resource2Amount);
                                PersonalResourceTotalAmount += request.TotalWithTaxes;
                                break;

                            case 1:
                                EquipmentRSPGAmount += request.GrantAmount;
                                EquipmentAvailableAmount += (request.Resource1Amount + request.Resource2Amount);
                                break;

                            case 2:
                                TravelRSPGAmount += request.GrantAmount;
                                TravelAvailableAmount += (request.Resource1Amount + request.Resource2Amount);
                                break;

                            case 3:
                                PersonalResourceRSPGAmount += request.GrantAmount;
                                PersonalResourceAvailableAmount += (request.Resource1Amount + request.Resource2Amount);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> OnGetDownload(int id, string filename)
        {
            Grant = await _context.Grant.Where(c => c.GrantID == id).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(filename) || !Grant.FileNames.Contains(filename))
            {
                return NotFound();
            }

            var filePath = Path.Combine("wwwroot/Uploads/" + id + "_" + Grant.GrantTitle + "/" + id + "_" + filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", filename);
        }
    }
}
