using System.ComponentModel.DataAnnotations;

namespace CS4760_Project.Models
{
    public class Committee
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Grant Type")]
        public string GrantType { get; set; }

    }
};