using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class Grant
    {
        // Basic Info
        public int GrantID { get; set; }
        public int GrantUserID { get; set; }

        [Display(Name = "Title")]
        public string GrantTitle { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }
        public int ReportID { get; set; }

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReportDate { get; set; }

        [Range(2024, int.MaxValue, ErrorMessage = "The year must be 2024 or later.")]
        public int Year { get; set; }

        // Grant Type and Program Details
        [Display(Name = "Type")]
        public string GrantType { get; set; }

        [Display(Name = "Sub Type")]
        public string GrantSubType { get; set; }

        [Display(Name = "Grant Program")]
        public string GrantProgram { get; set; }

        [Display(Name = "Principal Investigator")]
        public string PrincipalInvestigator { get; set; }

        [Display(Name ="Requires Animal or Human Subject")]
        public bool RequiresAnimalOrHumanSubject { get; set; }

        // Academic Information
        [Display(Name = "College")]
        public int GrantCollegeID { get; set; }

        [Display(Name = "Department")]
        public int GrantDepartmentID { get; set; }

        [Display(Name = "Mail Code")]
        public string MailCode { get; set; }

       // public string Account { get; set; }

        [Display(Name = "Undergraduate Student")]
        public bool IsUndergraduate { get; set; }

        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }

        // Chair Approval
        public bool IsChairApproved { get; set; }
        public bool IsChairRejected { get; set; }

        // Financial Information
        [Display(Name = "RSPG Amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal RSPGAmount { get; set; }

        [Display(Name = "Available Amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AvailableAmount { get; set; }

        [Display(Name = "Total Amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Amount Received")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal AmountReceived { get; set; }

        // Methodology and Timeline
        [Display(Name = "Procedure and Method")]
        public string ProcedureAndMethod { get; set; }
        public string Timeline { get; set; }

        // Financial Request Lists
        public List<int> PersonalResourceRequestIds { get; set; } = new List<int>();
        public List<int> EquipmentRequestIds { get; set; } = new List<int>();
        public List<int> TravelRequestIds { get; set; } = new List<int>();
        public List<int> OtherRequestIds { get; set; } = new List<int>();

        // Reviews and Files
        public List<int> ReviewIDs { get; set; } = new List<int>();

        [Display(Name ="Submitted for Review")]
        public bool IsAvailableForReview {  get; set; }
        public bool IsReviewed { get; set; }

        // File names
        public List<string> FileNames { get; set; } = new List<string>();
    }
}
