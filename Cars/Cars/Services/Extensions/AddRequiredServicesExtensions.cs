using System;
using Cars.Managers.Implementations;
using Cars.Managers.Interfaces;
using Cars.Models.DataModels;
using Cars.Services.EmailSender;
using Cars.Services.Implementations;
using Cars.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cars.Services.Extensions
{
    public static class AddRequiredServicesExtensions
    {
        public static void AddRequiredServices(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, 
                UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>>();
            
            services.AddScoped<IRecruitmentService, RecruitmentService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IFileUploadService, FileUploadService>();

            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<IAnalysisManager, AnalysisManager>();
            services.AddScoped<IRecruitmentManager, RecruitmentManager>();
            
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddCronJob<AnalysisHostedService>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"*/1 * * * *";
            });
            
            //Email Sender
            services.AddTransient<IEmailSender, EmailSender.EmailSender>();
            services.Configure<AuthMessageSenderOptions>(configuration);
            
            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }
    }
}