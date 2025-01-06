using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CS4760_Project.Data;
using CS4760_Project.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace CS4760_Project.Pages.RspgRubric
{
    public class CreateModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public CreateModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public User AuthenticatedUser { get; set; }

        [BindProperty]
        public Models.RspgRubric RspgRubric { get; set; } = default!;

        [BindProperty]
        public Models.Review Review { get; set; } = default!;

        public Models.Grant Grant { get; set; } = default!;

        [BindProperty]
        public int ReviewerId { get; set; }

        [BindProperty]
        public int GrantId { get; set; }

        [BindProperty]
        public int CommitteeId { get; set; }

        public string AreaOneSelectionError { get; set; } = string.Empty;
        public string AreaOneScoreError { get; set; } = string.Empty;
        public string AreaTwoSelectionError { get; set; } = string.Empty;
        public string AreaTwoScoreError { get; set; } = string.Empty;
        public string HemingwayError { get; set; } = string.Empty;

        [BindProperty]
        public bool SelectedHemingway { get; set; } 

        [BindProperty]
        public bool SelectedAreaOne { get; set; } = false;

        [BindProperty]
        public bool SelectedAreaTwo { get; set; } = false;

        public bool HemingwayAvailable { get; set; }

        public bool RequiresAnimalOrHumanSubject { get; set; }


        public async Task<IActionResult> OnGetAsync(int id, string grantProgram)
        {

            if (HttpContext.Session.GetString("UserData") != null)
            {
                AuthenticatedUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));

                ReviewerId = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData")).Id;
                
                GrantId = id;

                if(grantProgram.Contains("Hemingway"))
                {
                    HemingwayAvailable = true;
                }
                else
                {
                    HemingwayAvailable=false;
                }
                
                CommitteeId = (await _context.Committee.Where(c => c.UserId == ReviewerId).FirstOrDefaultAsync()).Id;
                
                Grant = await _context.Grant.Where(g => g.GrantID == GrantId).FirstOrDefaultAsync();
                RequiresAnimalOrHumanSubject = Grant.RequiresAnimalOrHumanSubject;
            }

            return Page();
        }


        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {


                return Page();
            }

            Review.Score = RspgRubric.TotalScore;

            _context.Review.Add(Review);
            _context.RspgRubric.Add(RspgRubric);

            await _context.SaveChangesAsync();

            Grant = await _context.Grant.Where(c => c.GrantID == Review.GrantId).FirstOrDefaultAsync();
            Grant.ReviewIDs.Add(Review.ReviewId);

            _context.Grant.Update(Grant);


            await _context.SaveChangesAsync();

            return RedirectToPage("/Grants/Review");
        }
    }
}
