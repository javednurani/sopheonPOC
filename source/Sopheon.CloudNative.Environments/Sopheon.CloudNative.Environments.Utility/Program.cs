using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sopheon.CloudNative.Environments.Utility.TestData;

[assembly: ExcludeFromCodeCoverage]
namespace Sopheon.CloudNative.Environments.Utility
{
   class Program
   {
      private static string _databaseConnection = "";
      public static IConfigurationRoot Configuration { get; set; }

      static async Task Main(string[] args)
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
         await TestDataHelper.SeedTestDataAsync(_databaseConnection);
      }
   }
}
