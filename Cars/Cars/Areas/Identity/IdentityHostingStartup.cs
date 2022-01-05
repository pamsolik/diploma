using Cars.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]

namespace Cars.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) => { });
    }
}