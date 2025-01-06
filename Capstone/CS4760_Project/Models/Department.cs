using System.ComponentModel.DataAnnotations;

namespace CS4760_Project.Models
{
    public class Department
    {

        public int DepartmentId { get; set; }

        [Display(Name = "Name")]

        public string DepartmentName { get; set; }

        [Display(Name = "Chair")]
        public string DepartmentChair { get; set; }

        [Display(Name ="Location")]
        public string DepartmentLocation { get; set; }


        // each department is associated with a college

        public int CollegeId {  get; set; }

        public string? College { get; set; }
    }
}
