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
    public static class AddAnalysisServiceExtensions
    {
        public static void AddAnalysisService(this IServiceCollection services, IConfiguration configuration)
        {
            var sonarConn = configuration.GetConnectionString("SonarConn").Split(';');
            if (sonarConn.Length < 4)
                throw new ArgumentException("SonarConn not configured properly 'basePath;sonarKey;user;password'");
            services.AddSingleton(_ =>
                new SonarQubeRequestHandler(sonarConn[0], sonarConn[1], sonarConn[2], sonarConn[3]));
            
            services.AddCronJob<AnalysisHostedService>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"*/1 * * * *";
            });
        }
    }
}