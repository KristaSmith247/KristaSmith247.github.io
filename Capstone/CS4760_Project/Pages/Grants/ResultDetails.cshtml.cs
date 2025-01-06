using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CS4760_Project.Pages.Grants
{
    public class ResultDetailsModel : PageModel
    {
        private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

        public ResultDetailsModel(CS4760_Project.Data.CS4760_ProjectContext context)
        {
            _context = context;
        }

        public Dictionary<Models.Committee, Review> CommitteeReview { get; set; }

        public Grant Grant { get; set; } = default!;

        public async Task OnGetAsync(int? id)
        {

            Grant = await _context.Grant.FirstOrDefaultAsync(g => g.GrantID == id);

            var reviews = await _context.Review.Where(r => Grant.ReviewIDs.Contains(r.ReviewId)).ToListAsync();

            var committeeIds = reviews.Select(r => r.CommitteeID).Distinct().ToList();

            var committees = await _context.Committee.Where(c => committeeIds.Contains(c.Id)).ToListAsync();

            CommitteeReview = new Dictionary<Models.Committee, Review>();

            foreach (var committee in committees)
            {
                var review = reviews.FirstOrDefault(r => r.CommitteeID == committee.Id);

                if (review != null)
                {
                    CommitteeReview.Add(committee, review);
                }
            }
        }
    }
}
