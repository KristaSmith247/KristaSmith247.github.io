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
using Newtonsoft.Json;
using System.Diagnostics;
using Azure.Core;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.InteropServices;
using MimeKit;
using MailKit.Net.Smtp;

namespace CS4760_Project.Pages.Grants
{
    public class CreateModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(CS4760_Project.Data.CS4760_ProjectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            this._environment = environment;
            
        }

        [BindProperty]
        public Grant Grant { get; set; } = default!;

        public User PrincipleInvestigator { get; set; }

        public SelectList PrincipleInvestigators { get; set; }
        public List<dynamic> AllUsersDynamic { get; set; } = new List<dynamic>();
        public SelectList GrantTypes { get; set; }
        public SelectList SubTypes { get; set; }
        public SelectList Colleges { get; set; }
        public SelectList Departments { get; set; }
        public List<dynamic> Programs { get; set; }
        public List<FinancialRequest> PersonalResourceRequests { get; set; }
        public List<FinancialRequest> EquipmentRequests { get; set; }
        public List<FinancialRequest> TravelRequests { get; set; }
        public List<FinancialRequest> OtherRequests { get; set; }

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

        // Uploaded Files
        [BindProperty, Display(Name = "Grant File")]
        public List<IFormFile> UploadedFile { get; set; }

        // Submit for Review
        [BindProperty]
        public bool IsAvailableForReview { get; set; } = false;


        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserData") != null)
            {
                PrincipleInvestigator = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
                
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

            Programs = grantProgramData;
            Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Departments = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "DepartmentName");
   
            var query = (from user in _context.User
                         select new
                         {
                             fullname = user.FirstName + " " + user.LastName,
                         });
            AllUsersDynamic = await query.ToListAsync<dynamic>();


            // personal resource requests
            var personalResourceSessionData = HttpContext.Session.GetString("PersonalResources");
            if (personalResourceSessionData == null)
            {
                PersonalResourceRequests = new List<FinancialRequest>();
            }
            else
            {
                PersonalResourceRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(personalResourceSessionData);
            }

            // Equipment Requests
            var equipmentSessionData = HttpContext.Session.GetString("Equipment");
            if (equipmentSessionData == null)
            {
                EquipmentRequests = new List<FinancialRequest>();
            }
            else
            {
                EquipmentRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(equipmentSessionData);
            }

            // Travel Requests
            var travelSessionData = HttpContext.Session.GetString("Travel");
            if (travelSessionData == null)
            {
                TravelRequests = new List<FinancialRequest>();
            }
            else
            {
                TravelRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(travelSessionData);
            }

            // Other Requests
            var otherSessionData = HttpContext.Session.GetString("Other");
            if (otherSessionData == null)
            {
                OtherRequests = new List<FinancialRequest>();
            }
            else
            {
                OtherRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(otherSessionData);
            }

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
            TotalAmount = PersonalResourceTotalAmount + EquipmentTotalAmount + TravelTotalAmount + OtherTotalAmount;

            return Page();
        }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Personal Resource Requests
            var personalResourceSessionData = HttpContext.Session.GetString("PersonalResources");
            if (string.IsNullOrEmpty(personalResourceSessionData))
            {
                PersonalResourceRequests = new List<FinancialRequest>();
            }
            else
            {
                PersonalResourceRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(personalResourceSessionData);
            }

            // Equipment Requests
            var equipmentSessionData = HttpContext.Session.GetString("Equipment");
            if (string.IsNullOrEmpty(equipmentSessionData))
            {
                EquipmentRequests = new List<FinancialRequest>();
            }
            else
            {
                EquipmentRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(equipmentSessionData);
            }

            // Travel Requests
            var travelSessionData = HttpContext.Session.GetString("Travel");
            if (string.IsNullOrEmpty(travelSessionData))
            {
                TravelRequests = new List<FinancialRequest>();
            }
            else
            {
                TravelRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(travelSessionData);
            }

            // Other Requests
            var otherSessionData = HttpContext.Session.GetString("Other");
            if (string.IsNullOrEmpty(otherSessionData))
            {
                OtherRequests = new List<FinancialRequest>();
            }
            else
            {
                OtherRequests = JsonConvert.DeserializeObject<List<FinancialRequest>>(otherSessionData);
            }

            if (HttpContext.Session.GetString("UserData") != null)
            {
                PrincipleInvestigator = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

            Grant.GrantUserID = PrincipleInvestigator.Id;


            _context.Grant.Add(Grant);
            await _context.SaveChangesAsync();

            AddFinancialRequests(PersonalResourceRequests, Grant.PersonalResourceRequestIds, Grant);
            AddFinancialRequests(EquipmentRequests, Grant.EquipmentRequestIds, Grant);
            AddFinancialRequests(TravelRequests, Grant.TravelRequestIds, Grant);
            AddFinancialRequests(OtherRequests, Grant.OtherRequestIds, Grant);

            _context.Grant.Update(Grant);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("PersonalResources");
            HttpContext.Session.Remove("Equipment");
            HttpContext.Session.Remove("Travel");
            HttpContext.Session.Remove("Other");
            HttpContext.Session.Remove("TempId");


            // UPLOAD FILES
            // get grant from above
            List<string> fileNameList = new List<string>();

            // check if uploads folder exists
            var path = Path.Combine(_environment.WebRootPath, "./Uploads");

            if (!Directory.Exists(path))
            {
                // if there is no uploads folder under the wwwroot folder, create one
                Directory.CreateDirectory(path);
            }


            // we want the grant path to be named after the grant title and id
            string myGrantPath = Grant.GrantID.ToString() + "_" + Grant.GrantTitle;

            string grantPath = Path.Combine(path, myGrantPath);
            if (!Directory.Exists(grantPath))
            {
                Directory.CreateDirectory(grantPath);
            }

            foreach (IFormFile file in UploadedFile)
            {
                fileNameList.Add(file.FileName);
                // file name is grant id and file
                string fileName = Grant.GrantID.ToString() + "_" + Path.GetFileName(file.FileName);
                using (FileStream stream = new FileStream(Path.Combine(grantPath, fileName), FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            // update grant files
            Grant.FileNames = fileNameList;
            _context.Grant.Update(Grant);
            await _context.SaveChangesAsync();

            if (Grant.IsAvailableForReview)
            {
                string subject = Grant.GrantType + " " + Grant.GrantSubType + " Grant approval for " + Grant.PrincipalInvestigator;
                string body = "You have a new grant to approve. <BR><B>Title:</B> " + Grant.GrantTitle + "<BR><B>Description:</B> " + Grant.Description;
                string from = PrincipleInvestigator.Email;

                SendEmail(subject, body, from);
            }

            return RedirectToPage("./Index");
        }

        private void SendEmail(string subject, string body, string from)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse("email"));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                client.Authenticate("email", "password");
                client.Send(email);
                client.Disconnect(true);
            }
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
                                OtherRSPGAmount += request.GrantAmount;
                                OtherAvailableAmount += (request.Resource1Amount + request.Resource2Amount);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void AddFinancialRequests(List<FinancialRequest> requests, List<int> requestIds, Grant grant)
        {
            foreach (var request in requests)
            {
                request.GrantID = grant.GrantID; // Assign GrantID
                request.TemporaryId = null; // Remove TempId
                _context.FinancialRequest.Add(request); // Add request to context
            }

            _context.SaveChanges();
            requestIds.AddRange(requests.Select(r => r.RequestId));
        }


    }
}
