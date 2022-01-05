using Cars.Services.Implementations;
using Core.DataModels;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Extensions;
using Services.Implementations;
using Services.Interfaces;
using Services.Managers.Implementations;
using Services.Managers.Interfaces;

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