using CS4760_Project.Data;
using CS4760_Project.Pages;
using CS4760_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using CS4760_Project.Pages.Colleges;

namespace GrantTest
{
    [TestClass]
    public class GrantCountTest
    {
        public CS4760_ProjectContext GetProjectContext()
        {
            DbContextOptions<CS4760_ProjectContext> options = new DbContextOptions<CS4760_ProjectContext>();

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder(options);

            SqlServerDbContextOptionsExtensions.UseSqlServer(builder, "Server=titan.cs.weber.edu,10433;Initial Catalog=4760_Group2;User ID=4760_Group2;Password=B3rmud@2;TrustServerCertificate=True;", null);

            var _context = new CS4760_ProjectContext((DbContextOptions<CS4760_ProjectContext>)builder.Options);

            return _context;
        }

        private readonly IWebHostEnvironment? _webHostEnvironment;

        public IWebHostEnvironment GetWebHostEnvironment()
        {
            return _webHostEnvironment;
        }


        [TestMethod]
        public void GrantCreationCountTest()
        {
            // this test will make sure the grant count is correct when a new grant is created

            Task.Run(async () =>
            {
                // set up
                CS4760_ProjectContext _context = GetProjectContext(); // get db context
                IWebHostEnvironment myEnvironment = GetWebHostEnvironment(); // get environment for file upload


                // get a list of all grants before you create a new one
                List<Grant> initialGrants = await _context.Grant.ToListAsync();

                int initialGrantCount = initialGrants.Count;

                // perform operations

                // add a new grant
                Grant newGrant = new Grant();
                newGrant.Year = 2025;
                newGrant.GrantProgram = "Research";
                newGrant.GrantUserID = 10; // Kira Thompson, chair@weber.edu
                newGrant.GrantID = 99999; // prevent grant duplication for now
                newGrant.GrantTitle = "Test";
                newGrant.Description = "Test Method Grant";
                newGrant.StartDate = DateTime.Now;
                newGrant.EndDate = DateTime.Now;
                newGrant.GrantType = "Fall";
                newGrant.GrantSubType = "Research";
                newGrant.PrincipalInvestigator = "Kira Thompson";
                newGrant.RequiresAnimalOrHumanSubject = false;
                newGrant.GrantCollegeID = 1; // I have no idea which college this is... EAST?
                newGrant.GrantDepartmentID = 1;
                newGrant.MailCode = "12345";
                newGrant.IsUndergraduate = false;
                newGrant.IsRejected = false;
                newGrant.IsApproved = false;
                newGrant.RSPGAmount = 10;
                newGrant.AvailableAmount = 10;
                newGrant.TotalAmount = 10;
                newGrant.ProcedureAndMethod = "Procedure";
                newGrant.Timeline = "A timeline";
                newGrant.IsAvailableForReview = false;
                newGrant.IsReviewed = false;

                // request id lists
                List<int> idList = new List<int>();
                idList.Add(1);
                newGrant.EquipmentRequestIds = idList;
                newGrant.TravelRequestIds = idList;
                newGrant.PersonalResourceRequestIds = idList;
                newGrant.OtherRequestIds = idList;
                newGrant.ReviewIDs = idList;
                List<string> files = new List<string>();
                files.Add("files");
                newGrant.FileNames = files;


                CS4760_Project.Pages.Grants.CreateModel m = new CS4760_Project.Pages.Grants.CreateModel(_context, myEnvironment); // need environment
                await _context.SaveChangesAsync(); // wait for the new grant to be created


                // check results by getting the new list of grants and making sure it matches the old amount + 1
                // assert grant count + 1
                List<Grant> updatedGrants = await _context.Grant.ToListAsync();
                int newGrantCount = updatedGrants.Count();

               Assert.AreEqual(newGrantCount, initialGrantCount + 1);

                // delete test grant
                if(newGrant != null && updatedGrants.Count > initialGrantCount)
                {
                    _context.Attach(newGrant);
                    _context.Grant.Remove(newGrant);
                    await _context.SaveChangesAsync();
                }

                // assert assignment count is back to pre-test level
                List<Grant> finalGrantsToCount = await _context.Grant.ToListAsync();
                int postDeletionCount = finalGrantsToCount.Count;

                Assert.AreEqual(postDeletionCount, initialGrantCount);


            });
        }

        [TestMethod]
        public void GrantUserTest()
        {
            // this test will make sure the grant count for a specific user is correct when a new grant is created

            Task.Run(async () =>
            {
                // set up
                CS4760_ProjectContext _context = GetProjectContext(); // get db context
                IWebHostEnvironment myEnvironment = GetWebHostEnvironment(); // get environment for file upload


                // get a list of all grants before you create a new one
                List<Grant> initialGrants = await _context.Grant.Where(g => g.GrantUserID == 10).ToListAsync();

                int initialGrantCount = initialGrants.Count;

                // perform operations

                // add a new grant
                Grant newGrant = new Grant();
                newGrant.Year = 2025;
                newGrant.GrantProgram = "Research";
                newGrant.GrantUserID = 10; // Kira Thompson, chair@weber.edu
                newGrant.GrantID = 99999; // prevent grant duplication for now
                newGrant.GrantTitle = "Test";
                newGrant.Description = "Test Method Grant";
                newGrant.StartDate = DateTime.Now;
                newGrant.EndDate = DateTime.Now;
                newGrant.GrantType = "Fall";
                newGrant.GrantSubType = "Research";
                newGrant.PrincipalInvestigator = "Kira Thompson";
                newGrant.RequiresAnimalOrHumanSubject = false;
                newGrant.GrantCollegeID = 1; 
                newGrant.GrantDepartmentID = 1;
                newGrant.MailCode = "12345";
                newGrant.IsUndergraduate = false;
                newGrant.IsRejected = false;
                newGrant.IsApproved = false;
                newGrant.RSPGAmount = 10;
                newGrant.AvailableAmount = 10;
                newGrant.TotalAmount = 10;
                newGrant.ProcedureAndMethod = "Procedure";
                newGrant.Timeline = "A timeline";
                newGrant.IsAvailableForReview = false;
                newGrant.IsReviewed = false;

                // request id lists
                List<int> idList = new List<int>();
                idList.Add(1);
                newGrant.EquipmentRequestIds = idList;
                newGrant.TravelRequestIds = idList;
                newGrant.PersonalResourceRequestIds = idList;
                newGrant.OtherRequestIds = idList;
                newGrant.ReviewIDs = idList;
                List<string> files = new List<string>();
                files.Add("files");
                newGrant.FileNames = files;


                CS4760_Project.Pages.Grants.CreateModel m = new CS4760_Project.Pages.Grants.CreateModel(_context, myEnvironment); // need environment
                await _context.SaveChangesAsync(); // wait for the new grant to be created


                // check results by getting the new list of grants for a specific user
                // and making sure it matches the old amount + 1
                List<Grant> updatedGrants = await _context.Grant.Where(g => g.GrantUserID == 10).ToListAsync();
                int newGrantCount = updatedGrants.Count();

                Assert.AreEqual(newGrantCount, initialGrantCount + 1);

                // delete test grant
                if (newGrant != null && updatedGrants.Count > initialGrantCount)
                {
                    _context.Attach(newGrant);
                    _context.Grant.Remove(newGrant);
                    await _context.SaveChangesAsync();
                }

                // assert assignment count is back to pre-test level
                List<Grant> finalGrantsToCount = await _context.Grant.ToListAsync();
                int postDeletionCount = finalGrantsToCount.Count;

                Assert.AreEqual(postDeletionCount, initialGrantCount);


            });
        }


        [TestMethod]
        public void GrantDeletionTest()
        {
            // this is to test if a grant is properly deleted after running unit tests
            Task.Run(async () =>
            {
                // set up
                CS4760_ProjectContext _context = GetProjectContext(); // get db context

                // get a list of grants that match the test id
                List<Grant> shouldBeEmpty = await _context.Grant.Where(g => g.GrantID == 99999).ToListAsync();
                List<Grant> isEmpty = new List<Grant>();

                Assert.IsTrue(shouldBeEmpty == isEmpty);
            });
        }

    }
}