using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Enums;
using Sopheon.CloudNative.Environments.Domain.Models;
using Environment = Sopheon.CloudNative.Environments.Domain.Models.Environment;

[assembly:ExcludeFromCodeCoverage]
namespace Sopheon.CloudNative.Environments.Utility
{

   class Program
   {
      private static string _databaseConnection = "";
      public static IConfigurationRoot Configuration { get; set; }
      static async System.Threading.Tasks.Task Main(string[] args)
      {

         if (args.Any(arg => arg == "-Database"))
         {
            _databaseConnection = System.Environment.GetEnvironmentVariable("LocalDatabaseConnectionString");
         }

         //if not running in pipeline, set _databaseConnection to value from user secrets
         if (string.IsNullOrEmpty(_databaseConnection))
         {
            var devEnvironmentVariable = System.Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
            var isDevelopment = string.IsNullOrEmpty(devEnvironmentVariable) || devEnvironmentVariable.ToLower() == "development";
            //Determines the working environment as IHostingEnvironment is unavailable in a console app

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (isDevelopment) //only add secrets in development
            {
               builder.AddUserSecrets<UserSecretManager>();
            }

            Configuration = builder.Build();

            IServiceCollection services = new ServiceCollection();
            services
               .Configure<UserSecretManager>(Configuration.GetSection(nameof(UserSecretManager)))
               .AddOptions()
               .AddLogging()
               .AddSingleton<ISecretRevealer, SecretRevealer>()
               .BuildServiceProvider();

            var serviceProvider = services.BuildServiceProvider();

            // Get the service you need - DI will handle any dependencies - in this case IOptions<SecretStuff>
            var revealer = serviceProvider.GetService<ISecretRevealer>();

            _databaseConnection = revealer.RevealLocalConnectionString();
         }

         DbContextOptions<EnvironmentContext> _dbContextOptions =
               new DbContextOptionsBuilder<EnvironmentContext>()
                  .UseSqlServer(_databaseConnection)
                  .Options;


         using var context = new EnvironmentContext(_dbContextOptions);

         if (!await context.Environments.AnyAsync())
         {
            DomainResourceType azureSqlResourceType = await context.DomainResourceTypes.FirstAsync(d => d.Id == (int)ResourceTypes.AzureSqlDb);

            Resource resource1 = new()
            {
               Uri = TestData.RESOURCE_URI_1,
               DomainResourceType = azureSqlResourceType
            };

            BusinessService businessService1 = new()
            {
               Name = TestData.BUSINESS_SERVICE_NAME_1
            };

            Environment environment1 = new()
            {
               Name = "Hammer Production",
               Description = "Hammer Corp production environment",
               EnvironmentKey = TestData.EnvironmentKey1,
               Owner = Guid.NewGuid(),
               IsDeleted = false
            };

            BusinessServiceDependency businessServiceDependency1 = new()
            {
               DependencyName = TestData.DEPENDENCY_NAME_1,
               BusinessService = businessService1,
               DomainResourceType = azureSqlResourceType
            };

            EnvironmentResourceBinding[] environmentResourceBindings = new EnvironmentResourceBinding[]
            {
               new EnvironmentResourceBinding
               {
                  Environment = environment1,
                  Resource = resource1,
                  BusinessServiceDependency = businessServiceDependency1
               },
            };

            context.EnvironmentResourceBindings.AddRange(environmentResourceBindings);
            int result = await context.SaveChangesAsync();
            Console.WriteLine(result + " entries written to the database.");
         }
         else
         {
            Console.WriteLine("0 entries written. Database is not empty.");
         }
      }
   }
}
