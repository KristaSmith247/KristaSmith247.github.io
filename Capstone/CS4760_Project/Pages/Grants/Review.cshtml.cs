using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CS4760_Project.Pages.Grants
{
    public class ReviewModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public ReviewModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }
        public IList<Grant> Grants { get; set; } = default!;
        public User User { get; set; }
        public Review Review { get; set; }
        public IList<Review> Reviews { get; set; } = default!;
        public Models.Committee Committee { get; set; } = default!;
        public async Task OnGetAsync()
        {
            //For Applicationing session between multiple pages
            if (HttpContext.Session.GetString("UserData") != null)
            {
                User = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("UserData"));
            }

            // Querys all grants and makes a list of grants that are available for review
            Grants = await _context.Grant.Where(g => g.IsAvailableForReview == true).ToListAsync();

            // Putting the ids' of the list of grants into the list reviewIds
            var reviewIds = Grants.SelectMany(g => g.ReviewIDs).Distinct().ToList();
            // Querys the Committee object based on the user's committee id
            Committee = await _context.Committee.Where(c => c.UserId == User.Id).FirstOrDefaultAsync();
            // What will happen if a Committee object does exists
            if (Committee != null)
            {
                // Querys a list of Reviews based on what is found on the list of review ids and if the user's committee id matches the Review object's committee id.
                Reviews = await _context.Review.Where(r => reviewIds.Contains(r.ReviewId) && r.CommitteeID == Committee.Id).ToListAsync();
            }
            // What will happen if there is no Committee object
            else
            {
                Reviews = new List<Review>();
            }
        }
    }
}
