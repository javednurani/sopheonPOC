using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sopheon.CloudNative.Products.Domain;

namespace Sopheon.CloudNative.Products.Functions
{
   public class Program
   {
      public static void Main()
      {
#if DEBUG
         Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
#endif
         var host = new HostBuilder()
            .ConfigureAppConfiguration(c => 
            {
               c.AddJsonFile("settings.json", optional: true, reloadOnChange: true);
            })
            .ConfigureFunctionsWorkerDefaults(workerApplication =>
            {
               // TODO: Other middleware; custom, order matters
               // https://joonasw.net/view/azure-ad-jwt-authentication-in-net-isolated-process-azure-functions?hmsr=joyk.com&utm_source=joyk.com&utm_medium=referral
               //builder.UseMiddleware<AuthenticationMiddleware>();
               //builder.UseMiddleware<AuthorizationMiddleware>();
               // Register our custom middleware with the worker
               workerApplication.UseMiddleware<AuthenticationMiddleware>();
               workerApplication.UseMiddleware<TenantEnvironmentMiddleware>();
            })
            .ConfigureServices(services =>
            {
               services.AddScoped<AzureAdJwtBearerValidation>();
               services.AddScoped<AuthenticationProvider>();

               services.AddScoped<IEnvironmentConnectionAuthorizer<FunctionContext>, FunctionContextEnvironmentAuthorizer>();

               services.AddScoped<ITenantEnvironmentConnectionStringProvider, FunctionContextConnectionStringProvider>();

               services.AddDbContext<ProductManagementContext>((serviceProvider, optionsBuilder) =>
               {
                  var connectionStringProvider = serviceProvider.GetService<ITenantEnvironmentConnectionStringProvider>();
                  optionsBuilder.UseSqlServer(connectionStringProvider.GetConnectionString());
               });
               //services.AddDbContextFactory<BloggingContext>(b =>
               //{
               //   b.UseInMemoryDatabase("TEST");
               //   //b.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Test")
               //   //
               //}, lifetime: ServiceLifetime.Scoped);
            })
      .Build();

         host.Run();
      }
   }
}