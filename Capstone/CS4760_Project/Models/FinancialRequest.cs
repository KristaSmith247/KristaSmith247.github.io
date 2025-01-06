using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class FinancialRequest
    {
        [Key]
        public int RequestId { get; set; }

        // Necessary TempId, for unique identification before adding to database
        public int? TemporaryId { get; set; }
        public int GrantID { get; set; }
        public string RequestType { get; set; }
        public string RequestTitle { get; set; }
        public string Resource1 { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Resource1Amount { get; set; }
        public string Resource2 { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Resource2Amount { get; set; }
        public string GrantType { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GrantAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? Tax { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalWithTaxes { get; set; }
        public bool IsStudent { get; set; }
    }
}
