#define Managed
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
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;

namespace Sopheon.CloudNative.Environments.Functions
{
   class Program
   {
      static Task Main(string[] args)
      {
         var host = new HostBuilder()
            .ConfigureAppConfiguration((hostContext, builder) =>
            {
               builder.AddCommandLine(args);
               if (hostContext.HostingEnvironment.IsDevelopment())
               {
                  builder.AddUserSecrets<Program>();
               }
               if (hostContext.HostingEnvironment.IsProduction())
               {
                  var keyVaultName = Environment.GetEnvironmentVariable("KeyVaultName");
                  var builtConfig = builder.Build();
                  var secretClient = new SecretClient(
                      new Uri($"https://{keyVaultName}.vault.azure.net/"),
                      new DefaultAzureCredential());
                  builder.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
               }
            })
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureServices((hostContext, services) =>
            {
               // Add Logging
               services.AddLogging();

               // Add HttpClient
               services.AddHttpClient();

               // Add Custom Services
               services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(hostContext.Configuration["EnvironmentsSqlConnectionString"]));
               services.AddAutoMapper(typeof(Program));
            })
            .Build();

         return host.RunAsync();
      }
   }
}
