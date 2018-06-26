using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace StockholderManager.Data.Seeds
{
  public class AdminRoleSeeder
  {
    internal async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env, Microsoft.Extensions.Options.IOptions<AdminSettings> settings, UserManager<Stockholder> userManager, RoleManager<IdentityRole> roleManager)
    {
      using (context)
      {
        context.Database.Migrate();

        try
        {
          if (!roleManager.Roles.Any())
          {
            var result = await roleManager.CreateAsync(new IdentityRole("gerant") { NormalizedName = "GERANT" });
            await roleManager.CreateAsync(new IdentityRole("societaire") { NormalizedName = "SOCIETAIRE" });
          }

          var adminRole = roleManager.Roles.First(f => f.Name == "gerant");
          var adminUser = await userManager.GetUsersInRoleAsync(adminRole.Name);
          if (!adminUser.Any())
          {
            var user = new Stockholder { FirstName = settings.Value.FirstName, LastName = settings.Value.LastName, UserName = settings.Value.Email, Email = settings.Value.Email };
            var userResult = await userManager.CreateAsync(user, settings.Value.Password);
            if (userResult.Succeeded)
            {
              await userManager.AddToRoleAsync(user, adminRole.Name);
            }
          }
        }
        catch (Exception)
        {

        }

      }
    }
  }
}
