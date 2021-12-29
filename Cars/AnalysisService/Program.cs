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
        
        var sonarConn = hostContext.Configuration.GetConnectionString("SonarConn").Split(';');
        if (sonarConn.Length < 4)
            throw new ArgumentException("SonarConn not configured properly 'basePath;sonarKey;user;password'");
        services.AddSingleton(_ =>
            new SonarQubeRequestHandler(sonarConn[0], sonarConn[1], sonarConn[2], sonarConn[3]));
        
        services.AddCronJob<AnalysisHostedService>(c =>
        {
            c.TimeZoneInfo = TimeZoneInfo.Local;
            c.CronExpression = @"*/1 * * * *";
        });
    })
    .Build();

await host.RunAsync();