using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Sopheon.CloudNative.Environments.Domain.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sopheon.CloudNative.Environments.Functions.Get
{
   class Program
   {
      readonly static string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString") ?? string.Empty;

      static Task Main(string[] args)
      {
         var host = new HostBuilder()
             .ConfigureAppConfiguration(configurationBuilder =>
             {
                configurationBuilder.AddCommandLine(args);
             })
             .ConfigureFunctionsWorkerDefaults()
             .ConfigureServices(services =>
             {
                // Add Logging
                services.AddLogging();

                // Add HttpClient
                services.AddHttpClient();

                // Add Custom Services
                services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(connectionString));
                services.AddAutoMapper(typeof(Program));
             })
             .Build();

         return host.RunAsync();
      }
   }
}
