using Cars.Models.DataModels;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Cars.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<RecruitmentApplication> Applications { get; set; }

        public DbSet<City> Cites { get; set; }

        public DbSet<CodeQualityAssessment> CodeQualityAssessments { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
    }
}