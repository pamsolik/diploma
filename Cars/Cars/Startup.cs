using System;
using System.IO;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Services.Extensions;
using Cars.Services.Other;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cars;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies()
                .UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection")));
        
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddDefaultUI();
            
        services.AddMvc()
            .AddRazorRuntimeCompilation();
        
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
        
        services.AddAuthentication().AddIdentityServerJwt();
  
        services.AddRequiredServices(Configuration);
        
        //services.AddAnalysisService(Configuration);

        services.AddRazorPages();

        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration => 
            { configuration.RootPath = "ClientApp/dist"; });
        
        services
            .AddControllersWithViews()
            .AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.Converters.Add(new StringEnumConverter());
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddControllersAsServices();
        
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.SlidingExpiration = true;
        });

        services.AddSwaggerDocument();

        RolesExtensions.InitializeAsync(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            // app.UseOpenApi();
            // app.UseSwaggerUi3();
            // app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            // // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.ConfigureCustomExceptionMiddleware();

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        if (!env.IsDevelopment()) app.UseSpaStaticFiles();

        app.UseRouting();
        
        app.UseIdentityServer();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller}/{action=Index}/{id?}");
            endpoints.MapRazorPages();
        });
        
        FileService.Create(Path.Combine(Directory.GetCurrentDirectory(), @"Resources"));
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            RequestPath = new PathString("/Resources")
        });
        
        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
            spa.Options.StartupTimeout = TimeSpan.FromSeconds(120);
            if (env.IsDevelopment())
                spa.UseAngularCliServer("start --disableHostCheck true");
            spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
        });
    }
}