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
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(options, operationalStoreOptions) { }
        
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<RecruitmentApplication> Applications { get; set; }

        public DbSet<City> Cities { get; set; }
        
        public DbSet<CodeQualityAssessment> CodeQualityAssessments { get; set; }

        public DbSet<CodeOverallQuality> CodeOverallQuality { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Experience> Experiences { get; set; }

        public DbSet<Education> Educations { get; set; }

        public DbSet<Skill> Skills { get; set; }
    }
}