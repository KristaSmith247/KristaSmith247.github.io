using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CS4760_Project.Models;

namespace CS4760_Project.Data
{
    public class CS4760_ProjectContext : DbContext
    {
        public CS4760_ProjectContext (DbContextOptions<CS4760_ProjectContext> options)
            : base(options)
        {
        }

        public DbSet<CS4760_Project.Models.User> User { get; set; } = default!;
        public DbSet<CS4760_Project.Models.Department> Department { get; set; } = default!;
        public DbSet<CS4760_Project.Models.College> College { get; set; } = default!;
        public DbSet<CS4760_Project.Models.Committee> Committee { get; set; } = default!;
        public DbSet<CS4760_Project.Models.Grant> Grant { get; set; } = default!;
        public DbSet<CS4760_Project.Models.FinancialRequest> FinancialRequest { get; set; } = default!;
        public DbSet<CS4760_Project.Models.Review> Review { get; set; } = default!;
        public DbSet<CS4760_Project.Models.RspgRubric> RspgRubric { get; set; } = default!;
        public DbSet<CS4760_Project.Models.GrantCriteria> GrantCriteria { get; set; } = default!;
        public DbSet<CS4760_Project.Models.RubricCriteria> RubricCriteria { get; set; } = default!;
        public DbSet<CS4760_Project.Models.Report> Report { get; set; } = default!;
    }
}
