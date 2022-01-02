using System;
using System.IO;
using Cars.Data;
using Cars.Models.DataModels;
using Cars.Services.Extensions;
using Cars.Services.Other;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

// Host.CreateDefaultBuilder(args)
//     .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
//     .Build()
//     .Run();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
        .UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));
        
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();
            
builder.Services.AddMvc()
    .AddRazorRuntimeCompilation();
        
builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
        
builder.Services.AddAuthentication().AddIdentityServerJwt();
  
builder.Services.AddRequiredServices(builder.Configuration);
        
//services.AddAnalysisService(Configuration);

builder.Services.AddRazorPages();

// In production, the Angular files will be served from this directory
builder.Services.AddSpaStaticFiles(configuration => 
    { configuration.RootPath = "ClientApp/dist"; });
        
builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(x =>
    {
        //x.SerializerSettings.Converters.Add(new StringEnumConverter());
        x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    })
    .AddControllersAsServices();
        
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.SlidingExpiration = true;
});

builder.Services.AddSwaggerDocument();

RolesExtensions.InitializeAsync(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
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
if (!app.Environment.IsDevelopment()) app.UseSpaStaticFiles();

app.UseRouting();
        
//app.UseIdentityServer();
//app.UseAuthorization();
        
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
    if (app.Environment.IsDevelopment())
        spa.UseAngularCliServer("start --disableHostCheck true");
    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
});

//app.MapFallbackToFile("index.html"); ;

app.Run();
