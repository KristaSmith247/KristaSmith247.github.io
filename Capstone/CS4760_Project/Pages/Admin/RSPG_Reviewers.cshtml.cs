using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CS4760_Project.Data;
using NuGet.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;


namespace CS4760_Project.Pages.Admin
{
    public class RSPG_ReviewersModel : PageModel
    {
        private CS4760_Project.Data.CS4760_ProjectContext _context;
        public RSPG_ReviewersModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }
        public User AuthenticatedUser { get; set; }
        public IList<User> User { get; set; } = default!;
        public IList<College> Colleges { get; set; } = default!;

        public IList<Department> Departments { get; set; } = default!;

        public IList<Models.Committee> Committees { get; set; } = default!;
        public List<dynamic> Users { get; set; } = new List<dynamic>();

        [BindProperty(SupportsGet = true)]
        public string FirstNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LastNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CollegeFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string DepartmentFilter { get; set; }

        [BindProperty]
        public Models.Committee Committee { get; set; }

        public IList<string> FilteredDepartments { get; set; } = new List<string>();
        public async Task OnGetAsync()
        {
            if (HttpContext.Session.GetString("UserData") != null)
            {
                AuthenticatedUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            } 

            Colleges = await _context.College.ToListAsync();

            var query = (from user in _context.User
                         join college in _context.College on user.CollegeId equals college.CollegeId
                         join department in _context.Department on user.DepartmentId equals department.DepartmentId
                         select new
                         {
                             IsAdmin = user.IsAdmin,
                             CommitteeId = user.CommitteeId,
                             Id = user.Id,
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             CollegeName = college.CollegeName,
                             DepartmentName = department.DepartmentName,
                             Committee = user.Committee,

                         });

            // Query Filters

            if (!string.IsNullOrWhiteSpace(FirstNameFilter))
            {
                query = query.Where(user => user.FirstName == FirstNameFilter);
            }

            if (!string.IsNullOrWhiteSpace(LastNameFilter))
            {
                query = query.Where(user => user.LastName == LastNameFilter);
            }

            if (!string.IsNullOrWhiteSpace(CollegeFilter))
            {
                query = query.Where(college => college.CollegeName == CollegeFilter);
                var departments = (from department in _context.Department
                                   where department.College == CollegeFilter
                                   select department.DepartmentName);

                FilteredDepartments = await departments.ToListAsync();
            }

            if (!string.IsNullOrWhiteSpace(DepartmentFilter))
            {
                query = query.Where(dept => dept.DepartmentName == DepartmentFilter);
            }

            Users = await query.ToListAsync<dynamic>();


        }

        public async Task<IActionResult> OnPostAddToCommittee(int userId, string fName, string lName)
        {
            var user = await _context.User.FindAsync(userId);


            Committee.GrantType = "RSPG";
            Committee.UserId = userId;
            Committee.FirstName = fName;
            Committee.LastName = lName;

            _context.Committee.Add(Committee);

            await _context.SaveChangesAsync();


            if (user != null)
            {


                user.CommitteeId = Committee.Id;
                user.Committee = "RSPG";

                await _context.SaveChangesAsync();

            }


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromCommittee(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            var committeeUser = await _context.Committee.FindAsync(userId);

            if (committeeUser != null)
            {
                _context.Committee.Remove(committeeUser);
                await _context.SaveChangesAsync();
            }

            if (user != null)
            {

                user.CommitteeId = null;
                user.Committee = null;
                await _context.SaveChangesAsync();

            }

            return RedirectToPage();

        }
    }
}
