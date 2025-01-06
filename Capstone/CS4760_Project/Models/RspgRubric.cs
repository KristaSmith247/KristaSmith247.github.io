using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class RspgRubric
    {
        public int RspgRubricID { get; set; }

        public int ReviewerId { get; set; } // authenticated user id
        public int GrantId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalScore { get; set; }

        [Required(ErrorMessage = "No Criterion Selected")]
        public string? AreaOneName { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? AreaOneScore { get; set; }
        public string? AreaOneComment { get; set; }

        [Required(ErrorMessage = "No Criterion Selected")]
        public string? AreaTwoName { get; set; }
        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? AreaTwoScore { get; set; }
        public string? AreaTwoComment { get; set; }


        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? MethodScore {  get; set; }
        public string? MethodComment { get; set; } 

        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? TimelineScore { get; set; }
        public string? TimelineComment { get; set; } 

        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? EvaluationScore { get; set; }
        public string? EvaluationComment { get; set; } 

        [Column(TypeName = "decimal(18,2)"), Required(ErrorMessage = "No Score Selected")]
        public decimal? DocumentScore { get; set; }
        public string? DocumentComment { get; set; } 


        [Column(TypeName = "decimal(18,2)")]
        public decimal? HemingwayScore { get; set; }
        public string? HemingwayComment { get;set; }  

    }
}
