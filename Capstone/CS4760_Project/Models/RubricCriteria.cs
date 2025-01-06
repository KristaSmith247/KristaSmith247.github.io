using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CS4760_Project.Models
{
    public class RubricCriteria
    {
        public int RubricCriteriaId { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        [Range(0, 100)]
        public int Weight { get; set; }

        [Range(0, 100)]
        [Display(Name = "Minimum")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumRange { get; set; } = 0;

        [Range(0, 100)]
        [Display(Name = "Maximum")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaximumRange { get; set; }

        [Range(0, 100)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Increment {  get; set; }


    }
}
