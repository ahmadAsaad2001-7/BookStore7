using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using StoreWebapi.Domain.Domain;

namespace StoreWebapi.Infrastructure;

public class IdentitySeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<user>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        
        if (!await roleManager.RoleExistsAsync("ADMINISTRATOR"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("ADMINISTRATOR"));

        var adminEmail = "admin@bookstore.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new user
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                imageUrl = "",   
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin@123456");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(adminUser, "ADMINISTRATOR");
        }
    }
}