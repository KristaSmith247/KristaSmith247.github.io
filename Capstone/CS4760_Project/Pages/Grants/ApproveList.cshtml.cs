using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Grants
{
    public class ApproveListModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public ApproveListModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }
        public IList<Grant> Grants { get; set; } = default!;
        public User User { get; set; }
        public async Task OnGetAsync()
        {
            //For Applicationing session between multiple pages
            if (HttpContext.Session.GetString("UserData") != null)
            {
                User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

            // Querys all grants and makes a list of grants that are available for Approval
            Grants = await _context.Grant.Where(g => g.IsChairApproved == false && g.IsChairRejected == false && g.GrantDepartmentID == User.DepartmentId).ToListAsync();
        }
    }
}
