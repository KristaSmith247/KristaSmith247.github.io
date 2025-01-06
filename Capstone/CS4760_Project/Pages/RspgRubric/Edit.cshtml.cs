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

namespace CS4760_Project.Pages.RspgRubric
{
    public class EditModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public EditModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.RspgRubric RspgRubric { get; set; } = default!;

        public Grant grant { get; set; }

        public User AuthenticatedUser {  get; set; }
        [BindProperty]
        public Models.Review Review { get; set; } = default!;


        [BindProperty]
        public int ReviewerId { get; set; }

        [BindProperty]
        public int GrantId { get; set; }

        [BindProperty]
        public int CommitteeId { get; set; }


        public bool HemingwayAvailable { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, int? grantId, int? userCommitteeId, int? userId, string? grantProgram)
        {
            if (HttpContext.Session.GetString("UserData") != null)
            {
                AuthenticatedUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

          
            // get by rubric id
            if (grantId == null || AuthenticatedUser.Id == null)
            {  
                    RspgRubric = await _context.RspgRubric.FirstOrDefaultAsync(m => m.ReviewerId == AuthenticatedUser.Id && m.GrantId == grantId);
                    grant = await _context.Grant.FirstOrDefaultAsync(g => g.GrantID == RspgRubric.GrantId); 
            }
            // get by grant and user id
            else
            {
                Models.RspgRubric findwithids = await _context.RspgRubric.FirstOrDefaultAsync(rs => (rs.GrantId == grantId) && (rs.ReviewerId == AuthenticatedUser.Id));

                var rspgrubric = await _context.RspgRubric.FirstOrDefaultAsync(m => m.RspgRubricID == findwithids.RspgRubricID);
                grant = await _context.Grant.FirstOrDefaultAsync(g => g.GrantID == grantId);
                GrantId = grant.GrantID; // use for manually sending in id
                Review = await _context.Review.FirstOrDefaultAsync(r => r.CommitteeID == AuthenticatedUser.CommitteeId && r.GrantId == grant.GrantID);
                
                if (rspgrubric == null)
                {
                    return NotFound();
                }

                RspgRubric = rspgrubric;
                RspgRubric.RspgRubricID = rspgrubric.RspgRubricID; // manually set rubric id

            }

            if (grant.GrantProgram.Contains("Hemingway"))
            {
                HemingwayAvailable = true;
            }
            else
            {
                HemingwayAvailable = false;
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Review.Score = RspgRubric.TotalScore; // update review score

            _context.Attach(RspgRubric).State = EntityState.Modified;
            //_context.Review.Update(Review);
           // _context.Attach(Review).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RspgRubricExists(RspgRubric.RspgRubricID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RspgRubricExists(int? id)
        {
            return _context.RspgRubric.Any(e => e.RspgRubricID == id);
        }
    }
}
