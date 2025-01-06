using CS4760_Project.Data;
using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CS4760_Project.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly CS4760_ProjectContext _context;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Report Report { get; set; } = new();
        public Grant Grant { get; set; }
        public Department Department { get; set; }

        [BindProperty, Display(Name = "Report File")]
        public IFormFile UploadedFile { get; set; }

        public CreateModel(CS4760_ProjectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Grant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();

            Department = await _context.Department.Where(d => d.DepartmentId == Grant.GrantDepartmentID).FirstOrDefaultAsync();


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Grant = await _context.Grant.Where(g => g.GrantID == id).FirstOrDefaultAsync();

            Report.GrantID = Grant.GrantID;
            Report.FileName = UploadedFile.FileName;

            _context.Report.Add(Report);
            await _context.SaveChangesAsync();

            Grant.ReportID = Report.ReportID;
            await _context.SaveChangesAsync();

            var path = Path.Combine(_environment.WebRootPath, "./Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string reportName = Grant.GrantID.ToString() + "_" + Report.ReportID.ToString() + "_" + "Report";
            string reportPath = Path.Combine(path, reportName);

            if (!Directory.Exists(reportPath))
            {
                Directory.CreateDirectory(reportPath);
            }

            string fileName = Grant.GrantID.ToString() + "_" + Report.ReportID.ToString() + "_" + Path.GetFileName(UploadedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(reportPath, fileName), FileMode.Create))
            {
                UploadedFile.CopyTo(stream);
            }

            return RedirectToPage("/Home");
        }
    }
}
