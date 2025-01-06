using CS4760_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;

namespace CS4760_Project.Pages.Grants
{
	public class ResultsModel : PageModel
	{
		private readonly CS4760_Project.Data.CS4760_ProjectContext _context;

		public ResultsModel(CS4760_Project.Data.CS4760_ProjectContext context)
		{
			_context = context;
		}

		public User User { get; set; }
		public Grant Grant { get; set; }	
		public Review Review { get; set; }
		public List<Review> ExcelReviews { get; set; } = default!;
		public List<Grant> Grants { get; set; } = new List<Grant>();
		public List<Grant> SurvivedGrants { get; set; } = new List<Grant>();
		public List<Grant> RejectedGrants { get; set; } = new List<Grant>();
		public List<Review> Reviews { get; set; } = new List<Review>();
		public List<GrantCriteria> Criteria { get; set; } = new List<GrantCriteria>();
		public Dictionary<Grant, decimal> GrantAverageScores { get; set; }
		public decimal RSPGAvailableAmount { get; set; } = 200000;

		public async Task OnGetAsync()
		{
			await LoadData();
			await UpdateGrantLists();
		}

		public async Task<IActionResult> OnPostApplyCutoff(decimal cutoff)
		{
			await LoadData();

			foreach (var grant in Grants)
			{
				if (GrantAverageScores.TryGetValue(grant, out decimal averageScore) && averageScore < cutoff)
				{
					grant.IsRejected = true;
					grant.IsApproved = false;
				}
				else
				{
					grant.IsRejected = false;
				}
			}

			await _context.SaveChangesAsync();
			await UpdateGrantLists();


			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostReset()
		{
			await LoadData();

			var allCriteria = await _context.GrantCriteria.ToListAsync();
			_context.GrantCriteria.RemoveRange(allCriteria);
			await _context.SaveChangesAsync();

			foreach (var grant in Grants)
			{
				grant.IsRejected = false;
				grant.IsApproved = false;
				grant.AmountReceived = 0;
			}

			await _context.SaveChangesAsync();
			await UpdateGrantLists();

			return RedirectToPage();
		}

		private async Task LoadData()
		{
			Grants = await _context.Grant.Where(g => g.ReviewIDs.Count > 0).ToListAsync();
			GrantAverageScores = new Dictionary<Grant, decimal>();

			foreach (var grant in Grants)
			{
				var reviews = await _context.Review.Where(r => grant.ReviewIDs.Contains(r.ReviewId)).ToListAsync();
				decimal? totalScore = reviews.Sum(r => r.Score);
				decimal averageScore = reviews.Count > 0 ? (decimal)totalScore / reviews.Count : 0;

				GrantAverageScores[grant] = averageScore;
			}
		}

		public async Task<IActionResult> OnPostAddCriteria(decimal cutoff1, decimal cutoff2, decimal percentageReceived)
		{
			await LoadData();

			Criteria = await _context.GrantCriteria.ToListAsync();

			foreach (var existingCriteria in Criteria)
			{
				if ((cutoff1 >= existingCriteria.Cutoff1 && cutoff1 <= existingCriteria.Cutoff2) ||
					(cutoff2 >= existingCriteria.Cutoff1 && cutoff2 <= existingCriteria.Cutoff2) ||
					(cutoff1 <= existingCriteria.Cutoff1 && cutoff2 >= existingCriteria.Cutoff2))
				{
					ModelState.AddModelError("CriteriaOverlap", "The selected criteria ranges overlap with an existing set. Please adjust the cutoff values.");
					await UpdateGrantLists();
					return Page();
				}
			}

			var criteria = new GrantCriteria
			{
				Cutoff1 = cutoff1,
				Cutoff2 = cutoff2,
				PercentageReceived = percentageReceived,
				GrantIds = new List<int>()
			};

			if (cutoff1 >= 0 && cutoff2 >= 0 && percentageReceived >= 0)
			{
				foreach (var grant in Grants.Where(g => !g.IsRejected))
				{
					if (GrantAverageScores.TryGetValue(grant, out var score) && score >= cutoff1 && score <= cutoff2)
					{
						grant.AmountReceived = grant.RSPGAmount * (percentageReceived / 100);
						grant.IsApproved = true;
						grant.ReportDate = DateTime.UtcNow.AddYears(2);
                        criteria.GrantIds.Add(grant.GrantID);
					}
				}
			}

			_context.GrantCriteria.Add(criteria);
			await _context.SaveChangesAsync();
			await UpdateGrantLists();

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostDistribute(decimal cutoff1, decimal cutoff2, decimal percentageReceived)
		{
			await LoadData();

			decimal DistributedTotal = 0m;

			var criteria = new GrantCriteria
			{
				Cutoff1 = cutoff1,
				Cutoff2 = cutoff2,
				PercentageReceived = null,
				GrantIds = new List<int>()
			};

			foreach (var grant in Grants.Where(g => !g.IsRejected))
			{
				if (GrantAverageScores.TryGetValue(grant, out var score))
				{
					if (score >= cutoff1 && score <= cutoff2)
					{
						DistributedTotal += grant.RSPGAmount;
					}
				}
			}

			foreach (var grant in Grants.Where(g => !g.IsRejected))
			{
				if (GrantAverageScores.TryGetValue(grant, out var score))
				{
					if (score >= cutoff1 && score <= cutoff2)
					{
						grant.AmountReceived = RSPGAvailableAmount >= DistributedTotal ? grant.RSPGAmount :
											   (grant.RSPGAmount / DistributedTotal) * RSPGAvailableAmount;
						grant.IsApproved = true;
                        grant.ReportDate = DateTime.UtcNow.AddYears(2);
						criteria.GrantIds.Add(grant.GrantID);
                    }
                }
			}

			_context.GrantCriteria.Add(criteria);
			await _context.SaveChangesAsync();
			await UpdateGrantLists();

			return RedirectToPage();
		}

		public async Task<IActionResult> OnPostGenerateSpreadsheet()
		{
			ExcelReviews = await _context.Review.ToListAsync();


			ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
			using (ExcelPackage excel = new ExcelPackage())
			{
				var worksheet = excel.Workbook.Worksheets.Add("Grant List");

				worksheet.Cells[1, 1].Value = "Index Number";
				worksheet.Cells[1, 2].Value = "Title";
				worksheet.Cells[1, 3].Value = "Investigator";
				worksheet.Cells[1, 4].Value = "RSPG Amount";
				worksheet.Cells[1, 5].Value = "Total Amount";

				int row = 2;
				foreach (var review in ExcelReviews)
				{
					Grant = await _context.Grant.Where(c => c.GrantID == review.GrantId).FirstOrDefaultAsync();
					User = await _context.User.Where(i => i.Id == Grant.GrantUserID).FirstOrDefaultAsync();
					if (Grant.IsApproved == true)
					{
						worksheet.Cells[row, 1].Value = User.IndexNum;
						worksheet.Cells[row, 2].Value = Grant.GrantTitle;
						worksheet.Cells[row, 3].Value = Grant.PrincipalInvestigator;
						worksheet.Cells[row, 4].Value = Grant.RSPGAmount;
						worksheet.Cells[row, 5].Value = Grant.TotalAmount;
						row++;
					}
				}
				var fileName = "GrantList.xlsx";
				var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

				var excelData = excel.GetAsByteArray();

				return File(excelData, contentType, fileName);
			}
		}

		private async Task UpdateGrantLists()
		{
			RejectedGrants = Grants.Where(g => g.IsRejected).ToList();
			SurvivedGrants = Grants.Where(g => !g.IsRejected && !g.IsApproved).ToList();

			decimal totalApprovedGrants = Grants.Where(g => g.IsApproved).Sum(g => g.AmountReceived);
			RSPGAvailableAmount -= totalApprovedGrants;

			Criteria = await _context.GrantCriteria.ToListAsync();
		}

		public async Task<IActionResult> OnPostDeleteCriteria(decimal cutoff1, decimal cutoff2, decimal percentage)
		{
			await LoadData();

			var criteriaToRemove = await _context.GrantCriteria
				.FirstOrDefaultAsync(c => c.Cutoff1 == cutoff1 && c.Cutoff2 == cutoff2 && c.PercentageReceived == percentage);

			if (criteriaToRemove != null)
			{
				foreach (var grantId in criteriaToRemove.GrantIds)
				{
					var grantToUpdate = _context.Grant.FirstOrDefault(g => g.GrantID == grantId);

					if (grantToUpdate != null)
					{
						grantToUpdate.IsRejected = false;
						grantToUpdate.IsApproved = false;
						grantToUpdate.AmountReceived = 0;
					}
				}

				_context.GrantCriteria.Remove(criteriaToRemove);
				await _context.SaveChangesAsync();
				await UpdateGrantLists();
			}

			return RedirectToPage();
		}
	}
}
