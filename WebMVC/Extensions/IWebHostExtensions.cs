using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StockholderManager.Extensions
{
  public static class IWebHostExtensions
  {
    public static IWebHost MigrateDbContext<TContext>(this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
      using (var scope = webHost.Services.CreateScope())
      {
        var services = scope.ServiceProvider;

        var logger = services.GetRequiredService<ILogger<TContext>>();

        var context = services.GetService<TContext>();

        try
        {
          logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");


          //if the sql server container is not created on run docker compose this
          //migration can't fail for network related exception. The retry options for DbContext only 
          //apply to transient exceptions.

          context.Database.Migrate();

          seeder(context, services);

          logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
        }
        catch (Exception ex)
        {
          logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
        }
      }

      return webHost;
    }
  }
}
