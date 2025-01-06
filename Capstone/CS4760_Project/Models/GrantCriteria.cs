using System.ComponentModel.DataAnnotations;

namespace CS4760_Project.Models
{
	public class GrantCriteria
	{
		[Key]
		public int CriteriaID { get; set; }

		[Range(0, 100)]
		public decimal Cutoff1 { get; set; }

		[Range(0, 100)]
		public decimal Cutoff2 { get; set; }

		[Range(0, 100)]
		public decimal? PercentageReceived { get; set; }

		public List<int> GrantIds { get; set; } = new List<int>();
	}
}
