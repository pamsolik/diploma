using Cars.Data;
using Cars.Managers.Implementations;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Services.Extensions;
using Cars.Services.Implementations;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
                .UseNpgsql(hostContext.Configuration.GetConnectionString("PostgreSQLConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IRecruitmentService, RecruitmentService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IFileUploadService, FileUploadService>();

        services.AddScoped<IAppUserManager, AppUserManager>();
        services.AddScoped<IAnalysisManager, AnalysisManager>();
        services.AddScoped<IRecruitmentManager, RecruitmentManager>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddAnalysisService(hostContext.Configuration);
    })
    .Build();

await host.RunAsync();