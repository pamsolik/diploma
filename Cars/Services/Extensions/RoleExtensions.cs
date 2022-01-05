using Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Services.Extensions;

public static class RolesExtensions
{
    public static void InitializeAsync(IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (var roleName in Enum.GetNames(typeof(UserRoles)))
        {
            var hasAdminRole = roleManager.RoleExistsAsync(roleName);
            hasAdminRole.Wait();

            if (hasAdminRole.Result) continue;

            var addRole = roleManager.CreateAsync(new IdentityRole(roleName));
            addRole.Wait();
        }
    }
}