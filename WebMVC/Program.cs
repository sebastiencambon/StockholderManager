using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StockholderManager.Data;
using StockholderManager.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StockholderManager.Data.Seeds;
using Microsoft.AspNetCore.Identity;

namespace StockholderManager
{
  public class Program
  {
    public static void Main(string[] args)
    {
      BuildWebHost(args).MigrateDbContext<ApplicationDbContext>((context, services) =>
      {
        var env = services.GetService<IHostingEnvironment>();
        var settings = services.GetService<IOptions<AdminSettings>>();
        //var logger = services.GetService<ILogger<OrderingContextSeed>>();

        new AdminRoleSeeder()
            .SeedAsync(context, env, settings, services.GetService<UserManager<Stockholder>>(), services.GetService<RoleManager<IdentityRole>>())
            .Wait();
      }).Run();
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
  }
}
