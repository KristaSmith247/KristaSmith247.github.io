using CS4760_Project.Data;
using CS4760_Project.Enums;
using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CS4760_Project.Pages.Users
{
    public class SignupModel : PageModel
    {
        public readonly CS4760_ProjectContext _context;

        public SignupModel(CS4760_ProjectContext context)
        {
            _context = context;
        }

        // Signup Model Properties
        [BindProperty]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BindProperty]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [BindProperty]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [BindProperty]
        [Display(Name = "W Number"), MinLength(8)]

        public string WNumber { get; set; }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public int IndexNum { get; set; }

        [BindProperty]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; } = false;

        [BindProperty]
        [Display(Name = "College")]
        public int? CollegeId { get; set; }

        [BindProperty]
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        [BindProperty]
        [Display(Name = "Faculty Role")]
        public FacultyRole? FacultyRole { get; set; }

        // Select Lists for Sign up page
      //  public SelectList Colleges { get; set; }
        public SelectList Departments { get; set; }
        public SelectList FacultyRoles { get; set; }

        // test stuff
        public IList<Department> FilteredDepartments { get; set; } = new List<Department>()!;
        public IList<College> Colleges { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string SelectedCollege { get; set; } = default!;

        // Boolean to see if Admin exists in database
        public bool AdminExists { get; set; }

        public async Task<IActionResult> OnGet()
        {
            AdminExists = _context.User.Any(u => u.IsAdmin);

            // Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Colleges = await _context.College.ToListAsync();

            FilteredDepartments =  await _context.Department.ToListAsync();

            Departments = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "DepartmentName");
            

            FacultyRoles = new SelectList(Enum.GetValues(typeof(FacultyRole)).Cast<FacultyRole>());

            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            // After error checking set AdminExists, and SelectLists again
            AdminExists = _context.User.Any(u => u.IsAdmin);

            //Colleges = new SelectList(await _context.College.ToListAsync(), "CollegeId", "CollegeName");
            Colleges = await _context.College.ToListAsync();
            Departments = new SelectList(await _context.Department.ToListAsync(), "DepartmentId", "DepartmentName");
            FacultyRoles = new SelectList(Enum.GetValues(typeof(FacultyRole)).Cast<FacultyRole>());

            // Check if the passwords match
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            // Check if there's already an admin
            if (IsAdmin && _context.User.Any(u => u.IsAdmin))
            {
                ModelState.AddModelError(string.Empty, "There is already an admin.");
                return Page();
            }

            // Check if there is an existing Dean for the college

            bool deanExists = _context.User.Any(u => u.FacultyRole == Enums.FacultyRole.Dean && u.CollegeId == CollegeId);
            if (FacultyRole == Enums.FacultyRole.Dean && deanExists)
            {
                ModelState.AddModelError(string.Empty, "There is already a dean for this college.");
                return Page();
            }

            // Check if there is an existing Chair for the college department
            bool chairExists = _context.User.Any(u => u.FacultyRole == Enums.FacultyRole.Chair && u.CollegeId == CollegeId && u.DepartmentId == DepartmentId);
            if (FacultyRole == Enums.FacultyRole.Chair && chairExists)
            {
                ModelState.AddModelError(string.Empty, "There is already a chair for this college's department.");
                return Page();
            }

            // If admin, make sure college fields are null
            if (IsAdmin)
            {
                CollegeId = null;
                DepartmentId = null;
                FacultyRole = null;
            }

            // If dean, make sure department is null
            if (FacultyRole == Enums.FacultyRole.Dean)
            {
                DepartmentId = null;
            }

            // Hash Password
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            // Create new user for Database
            var newUser = new User
            {
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                WNumber = WNumber,
                IndexNum = IndexNum,
                Password = hashedPassword,
                IsAdmin = IsAdmin,
                CollegeId = CollegeId,
                DepartmentId = DepartmentId,
                FacultyRole = FacultyRole,
            };

            if (FacultyRole == Enums.FacultyRole.Dean)
            {
                var college = _context.College.Where(c => c.CollegeId == CollegeId).FirstOrDefault();

                if (college != null)
                {
                    college.DeanId = newUser.Id;
                    _context.College.Update(college);
                }
            }

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Users/Login");
        }

    }
}
