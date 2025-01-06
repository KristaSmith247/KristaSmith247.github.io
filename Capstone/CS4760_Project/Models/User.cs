using CS4760_Project.Enums;

namespace CS4760_Project.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string WNumber { get; set; }
        public bool IsAdmin { get; set; }

        // Nullable Properties
        public int? CollegeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CommitteeId { get; set; }
        public string? Committee {  get; set; }
        public int? IndexNum { get; set; }
        public FacultyRole? FacultyRole { get; set; }
    }
}
