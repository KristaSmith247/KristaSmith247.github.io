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
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Grants
{
    public class EditModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public EditModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Grant Grant { get; set; } = default!;
        public User User { get; set; }
        public Department Department { get; set; }
        public College College { get; set; }
        public Models.Committee Committee { get; set; }
        public SelectList GrantTypes { get; set; }
        public SelectList SubTypes { get; set; }
        public List<Review> Reviews { get; set; }
        public SelectList Colleges { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Programs { get; set; }
        public string CollegeName { get; set; }
        public string DepartmentName { get; set; }
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
            if (id == null)
            {
                return NotFound();
            }

            var grant = await _context.Grant.FirstOrDefaultAsync(m => m.GrantID == id);
            if (grant == null)
            {
                return NotFound();
            }

            Grant = grant;

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

            Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Departments = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "DepartmentName");

            CollegeName = await _context.College
                .Where(c => c.CollegeId == Grant.GrantCollegeID)
                .Select(c => c.CollegeName)
                .FirstOrDefaultAsync();

            DepartmentName = await _context.Department
                .Where(d => d.DepartmentId == Grant.GrantDepartmentID)
                .Select(d => d.DepartmentName)
                .FirstOrDefaultAsync();

            if (HttpContext.Session.GetString("UserData") != null)
            {
                User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

            var grantTypeData = new List<dynamic>
            {
                new { Id = "RSPG", Name = "RSPG"},
                //new { Id = "ARCC", Name = "ARCC"},
                //new { Id = "OUR", Name = "OUR" },
            };

            var grantSubTypeData = new List<dynamic>
            {
                new { Id = "Travel", Name = "Travel"},
                new { Id = "Fall", Name = "Fall"  },
                new { Id = "Spring", Name = "Spring"},
            };

            var grantProgramData = new List<dynamic>
            {
                new { Id = "Research", Name = "Research"},
                new { Id = "Hemingway Collaborative Awards", Name = "Hemingway Collaborative Awards"  },
                new { Id = "Hemingway Excellence Awards", Name = "Hemingway Excellence Awards"},
                new { Id = "Hemingway Adjunct Faculty Grants", Name = "Hemingway Adjunct Faculty Grants"},
                new { Id = "Hemingway Faculty Vitality Grants", Name = "Hemingway Faculty Vitality Grants"},
                new { Id = "Hemingway New Faculty Grants", Name = "Hemingway New Faculty Grants"},
                new { Id = "Innovative Teaching Grants", Name = "Innovative Teaching Grants"},
                new { Id = "Research and Professional Grants", Name = "Research and Professional Grants"},
            };

            GrantTypes = new SelectList(grantTypeData, "Id", "Name");
            SubTypes = new SelectList(grantSubTypeData, "Id", "Name");
            Programs = new SelectList(grantProgramData, "Id", "Name");

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            // Retrieve the existing grant to avoid overwriting readonly fields
            var existingGrant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();

            // Update only editable fields
            existingGrant.GrantTitle = Grant.GrantTitle;
            existingGrant.GrantType = Grant.GrantType;
            existingGrant.GrantSubType = Grant.GrantSubType;
            existingGrant.GrantCollegeID = Grant.GrantCollegeID;
            existingGrant.GrantDepartmentID = Grant.GrantDepartmentID;
            existingGrant.GrantProgram = Grant.GrantProgram;
            existingGrant.PrincipalInvestigator = Grant.PrincipalInvestigator;
            existingGrant.IsUndergraduate = Grant.IsUndergraduate;
            existingGrant.MailCode = Grant.MailCode;
            existingGrant.Description = Grant.Description;
            existingGrant.ProcedureAndMethod = Grant.ProcedureAndMethod;
            existingGrant.Timeline = Grant.Timeline;

            // Save changes to the database
            await _context.SaveChangesAsync();
            
            return RedirectToPage("/Grants/Index");
        }

        private bool GrantExists(int id)
        {
            return _context.Grant.Any(e => e.GrantID == id);
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
    }
}
