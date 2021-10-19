#define Managed
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Queries;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;

namespace Sopheon.CloudNative.Environments.Functions
{
	[ExcludeFromCodeCoverage]
   class Program
   {
      private static Lazy<IAzure> _lazyAzureClient = new Lazy<IAzure>(GetAzureInstance);

      static Task Main(string[] args)
      {
         IHost host = new HostBuilder()
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
            .ConfigureOpenApi()
            .ConfigureServices((hostContext, services) =>
            {
               // Add Logging
               services.AddLogging();

               // Add HttpClient
               services.AddHttpClient();

               // Add Custom Services
               string connString = string.Empty;
               if (hostContext.HostingEnvironment.IsProduction())
               {
                  connString = Environment.GetEnvironmentVariable("SQLCONNSTR_EnvironmentsSqlConnectionString");
               }
               if (hostContext.HostingEnvironment.IsDevelopment())
               {
                  connString =
                     hostContext.Configuration["SQLCONNSTR_EnvironmentsSqlConnectionString"] ??          // local dev
                     Environment.GetEnvironmentVariable("SQLCONNSTR_EnvironmentsSqlConnectionString");   // CI pipeline
               }
               services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(connString));
               services.AddAutoMapper(typeof(Program));

               services.AddScoped<IEnvironmentRepository, EFEnvironmentRepository>();
               services.AddScoped<IEnvironmentQueries, EFEnvironmentQueries>();
               services.AddScoped<IValidator<EnvironmentDto>, EnvironmentDtoValidator>();
               services.AddScoped<IRequiredNameValidator, RequiredNameValidator>();
               services.AddScoped<IDatabaseBufferMonitorHelper, DatabaseBufferMonitorHelper>();
               services.AddScoped<HttpResponseDataBuilder>();
               services.AddScoped<IAzure>(sp => _lazyAzureClient.Value);   // single instance shared across functions
            })
            .Build();

         return host.RunAsync();
      }

      private static IAzure GetAzureInstance()
      {
         // authenticate with Service Principal credentials
         //logger.LogInformation("Fetching Service Principal credentials");
         string clientId = Environment.GetEnvironmentVariable("AzSpClientId");
         string clientSecret = Environment.GetEnvironmentVariable("AzSpClientSecret");
         string tenantId = Environment.GetEnvironmentVariable("AzSpTenantId");

         AzureCredentials credentials = SdkContext.AzureCredentialsFactory
            .FromServicePrincipal(clientId, clientSecret, tenantId, environment: AzureEnvironment.AzureGlobalCloud);

         //logger.LogInformation($"Authenticating with Azure...");

         return Microsoft.Azure.Management.Fluent.Azure
            .Authenticate(credentials)
            .WithDefaultSubscription();	// TODO: can we use the async method?  having trouble consuming in Main
      }
   }
}
