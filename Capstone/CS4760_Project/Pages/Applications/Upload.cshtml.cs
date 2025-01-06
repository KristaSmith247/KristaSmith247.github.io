using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using CS4760_Project.Models;
using CS4760_Project.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;


namespace CS4760_Project.Pages.Applications
{
    public class UploadModel :PageModel
    {
        // get the context to query for GrantID
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;
      
        private IWebHostEnvironment _environment; // needed to provide info about web environment do not remove

		//public User AuthenticatedUser { get; set; } // user logged in

        public Grant MyGrant { get; set; }

        
		public UploadModel(IWebHostEnvironment environment, CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _environment = environment; // use for web environment info
            _context = context; // database context
            MyGrant = new Grant();
        }

		
    public void OnGet()
    {
    }

    public void OnPostUpload(List<IFormFile> addedFiles)
		{
			string path = Path.Combine(this._environment.WebRootPath, "Uploads");

			if (!Directory.Exists(path))
            {
                // if there is no uploads folder under the wwwroot folder, create one
                Directory.CreateDirectory(path);
            }

            
            // we want the grant path to be named after the grant title and id

            string myGrantPath = MyGrant.GrantTitle + MyGrant.GrantID.ToString();

            string test = MyGrant.GrantID.ToString();
            string grantPath = Path.Combine(path, test);
            if (!Directory.Exists(grantPath))
            {
                Directory.CreateDirectory(grantPath);
            }

            foreach (IFormFile file in addedFiles) 
            { 
                string fileName = Guid.NewGuid().ToString("N").Substring(0, 5) + "_" + Path.GetFileName(file.FileName);
				using (FileStream stream = new FileStream(Path.Combine(grantPath, fileName), FileMode.Create))
				{
					file.CopyTo(stream);
                }
            }
        }
    }
}
