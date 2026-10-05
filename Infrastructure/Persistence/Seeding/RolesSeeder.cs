using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;
namespace Persistence.Seeding
{
    public class RolesSeeder(RoleManager<IdentityRole> roleManager)
    {
        public async Task SeedAsync()
        {
            var roles = Enum.GetNames(typeof(Roles));
            foreach (var roleName in roles)
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}
