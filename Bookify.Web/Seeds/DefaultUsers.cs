﻿using Microsoft.AspNetCore.Identity;

namespace Bookify.Web.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager)
    {

        ApplicationUser admin = new()
        {
            UserName = "admin",
            Email = "shehabw126@gmail.com",
            FullName = "Admin",
            EmailConfirmed = true
        };

        var user = await userManager.FindByEmailAsync(admin.Email);

        if (user is null)
        {
            await userManager.CreateAsync(admin,"ShehabWael832.Info");
            await userManager.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }
}
