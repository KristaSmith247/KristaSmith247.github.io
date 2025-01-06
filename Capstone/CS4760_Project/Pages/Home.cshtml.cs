using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Xml;

namespace CS4760_Project.Pages
{
    public class HomeModel : PageModel
    {

		private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public User AuthenticatedUser { get; set; }

        public IList<Models.Grant> GrantList { get; set; } = default!; // list of unreviewed grants
        public IList<Models.Grant> ReviewedGrantList { get; set; } = default!; // list of reviewed grants
		public IList<Models.Grant> NeedsApprovalGrantList { get; set; } = default!; // list of potential grants
		public IList<Models.Grant> ApprovedGrantList { get; set; } = default!; // list of approved grants

        public HomeModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
            GrantList = new List<Grant>();
            ReviewedGrantList = new List<Grant>();
            NeedsApprovalGrantList = new List<Grant>();

        }

        public async Task OnGetAsync()
        {
            //For Applicationing session between multiple pages
            if (HttpContext.Session.GetString("UserData") != null)
            {
                AuthenticatedUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));

                if (AuthenticatedUser != null)
                {
                    var principalUser = AuthenticatedUser.FirstName + " " + AuthenticatedUser.LastName;
                    GrantList = await _context.Grant.Where(g => principalUser == g.PrincipalInvestigator && g.IsAvailableForReview != false).ToListAsync();
                    ReviewedGrantList = await _context.Grant.Where(g => principalUser == g.PrincipalInvestigator && g.IsReviewed != false).ToListAsync();
                    NeedsApprovalGrantList = await _context.Grant.Where(g => g.IsChairApproved == false && g.IsChairRejected == false && g.GrantDepartmentID == AuthenticatedUser.DepartmentId).ToListAsync();
                    ApprovedGrantList = await _context.Grant.Where(g => principalUser == g.PrincipalInvestigator && g.IsApproved == true).ToListAsync();
                }
            }


        }

    }
}
