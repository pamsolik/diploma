using System;
using System.IO;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Models.Enums;
using Cars.Services.EmailSender;
using Cars.Services.Extensions;
using Cars.Services.Implementations;
using Cars.Services.Interfaces;
using Cars.Services.Other;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Cars
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseLazyLoadingProxies()
                        .UseNpgsql(Configuration.GetConnectionString("PostgreSQLConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();
            
            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
                    options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

            services.AddRequiredServices(Configuration);

            services.AddAuthentication().AddIdentityServerJwt();

            services.AddControllersWithViews();
            services.AddRazorPages();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => 
                { configuration.RootPath = "ClientApp/dist"; });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.SlidingExpiration = true;
            });
            
            //Cookies and data protection token lifespan

            // services.ConfigureApplicationCookie(o => {
            //     o.ExpireTimeSpan = TimeSpan.FromDays(5);
            //     o.SlidingExpiration = true;
            // });
            //
            // services.Configure<DataProtectionTokenProviderOptions>(o => 
            //     o.TokenLifespan = TimeSpan.FromHours(3));

            services.AddSwaggerDocument();
            
            RolesExtensions.InitializeAsync(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseOpenApi();
                app.UseSwaggerUi3();
                //app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                // app.UseExceptionHandler("/Error");
                // // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //app.ApplicationServices.GetService<AnalysisService>();

            //app.ConfigureExceptionHandler();
            app.ConfigureCustomExceptionMiddleware();

            //TODO: Move to prod
            //app.UseExceptionHandler("/api/error");
            // app.UseExceptionHandler(c => c.Run(async context =>
            // {
            //     var exception = context.Features
            //         .Get<IExceptionHandlerPathFeature>()
            //         .Error;
            //     var response = new ErrorDetails(exception);
            //     await context.Response.WriteAsJsonAsync(response);
            // }));

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment()) app.UseSpaStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });


            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";
                spa.Options.StartupTimeout = TimeSpan.FromSeconds(120);
                if (env.IsDevelopment())
                    spa.UseAngularCliServer("start");
                //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
            });
        }
    }
}