using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class Report
    {
        [Key]
        public int ReportID { get; set; }
        public int GrantID { get; set; }

        [Display(Name = "Project Director")]
        public string ProjectDirector { get; set; }

        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }

        [Display(Name = "Other Participants")]
        public string OtherParticipants { get; set; }

        [Display(Name = "Award Date")]
        public DateTime AwardDate { get; set; }

        [Display(Name = "Completion Date")]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Amount of RSPG Award")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RSPGAmount { get; set; }

        [Display(Name = "Proposal Type")]
        public string ProposalType { get; set; }

        [Display(Name = "Project Purpose")]
        public string ProjectPurpose { get; set; }

        [Display(Name = "Project Outcomes")]
        public string ProjectOutcomes { get; set; }

        [Display(Name = "Evaluation and Dissemination Outcomes")]
        public string EvaluationDisseminationOutcomes { get; set; }
        public string FileName { get; set; }
    }
}
