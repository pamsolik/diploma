using Core.DataModels;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;

    public DbSet<RecruitmentApplication> Applications { get; set; } = null!;

    public DbSet<City> Cities { get; set; } = null!;

    public DbSet<CodeQualityAssessment> CodeQualityAssessments { get; set; } = null!;

    public DbSet<CodeOverallQuality> CodeOverallQuality { get; set; } = null!;
    public DbSet<Recruitment> Recruitments { get; set; } = null!;

    public DbSet<Project> Projects { get; set; } = null!;

    public DbSet<Experience> Experiences { get; set; } = null!;

    public DbSet<Education> Educations { get; set; } = null!;

    public DbSet<Skill> Skills { get; set; } = null!;
}