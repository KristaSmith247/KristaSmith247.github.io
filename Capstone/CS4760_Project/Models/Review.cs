using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int GrantId { get; set; }
        public int CommitteeID { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Score { get; set; }

    }
}
