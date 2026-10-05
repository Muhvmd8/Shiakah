using Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Shared.Enums;

namespace Persistence.Seeding
{
    public class SuperAdminSeeder(
        UserManager<ApplicationUser> userManager
        )
    {
        public async Task SeedAsync()
        {
            var superAdminUser = await userManager.FindByNameAsync("superadmin");
            if (superAdminUser == null)
            {
                superAdminUser = new ApplicationUser
                {
                    UserName = "superadmin",
                    Email = "superadmin@shiakah.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(superAdminUser, "P@ssword12");
                if (!result.Succeeded)
                    throw new Exception("Failed to create SuperAdmin user");
            }

            if (!await userManager.IsInRoleAsync(superAdminUser, Roles.SuperAdmin.ToString()))
            {
                await userManager.AddToRoleAsync(superAdminUser, Roles.SuperAdmin.ToString());
            }
            if (!await userManager.IsInRoleAsync(superAdminUser, Roles.Admin.ToString()))
            {
                await userManager.AddToRoleAsync(superAdminUser, Roles.Admin.ToString());
            }
        }
    }
}
