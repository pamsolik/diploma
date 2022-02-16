using Core.DataModels;
using Infrastructure.EmailSender;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Implementations;
using Services.Interfaces;
using Services.Managers.Implementations;
using Services.Managers.Interfaces;

namespace Services.Extensions;

public static class AddRequiredServicesExtensions
{
    public static void AddRequiredServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,
            UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();

        services.AddScoped<IRecruitmentService, RecruitmentService>();
        services.AddScoped<IProjectsService, ProjectsService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IFileUploadService, FileUploadService>();

        services.AddScoped<IAppUserManager, AppUserManager>();
        services.AddScoped<IAnalysisManager, AnalysisManager>();
        services.AddScoped<IRecruitmentManager, RecruitmentManager>();
        services.AddScoped<IProjectsManager, ProjectsManager>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        //Email Sender
        services.AddTransient<IEmailSender, EmailSender>();
        services.Configure<AuthMessageSenderOptions>(configuration);

        services.Configure<FormOptions>(o =>
        {
            o.ValueLengthLimit = int.MaxValue;
            o.MultipartBodyLengthLimit = int.MaxValue;
            o.MemoryBufferThreshold = int.MaxValue;
        });
    }
}