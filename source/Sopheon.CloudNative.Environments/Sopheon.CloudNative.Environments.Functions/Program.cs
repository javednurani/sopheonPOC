#define Managed
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
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
using Sopheon.CloudNative.Environments.Domain.Commands;
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
      private static Lazy<IAzure> _lazyAzureClient;

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
               services.AddHttpClient(StringConstants.HTTP_CLIENT_NAME_AZURE_REST_API, (servProd, client) => ConfigureAzureRestApiClient(client));

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
               services.AddScoped<IEnvironmentCommands, EFEnvironmentCommands>();
               services.AddScoped<IValidator<EnvironmentDto>, EnvironmentDtoValidator>();
               services.AddScoped<IValidator<ResourceRegistrationDto>, ResourceRegistrationDtoValidator>();              
               services.AddScoped<IRequiredNameValidator, RequiredNameValidator>();
               services.AddScoped<IDatabaseBufferMonitorHelper, DatabaseBufferMonitorHelper>();
               services.AddScoped<IResourceAllocationHelper, ResourceAllocationHelper>();
               services.AddScoped<HttpResponseDataBuilder>();
               
               services.AddSingleton<Lazy<IAzure>>(sp => new Lazy<IAzure>(GetAzureInstance(hostContext)));   // single instance shared across functions
            })
            .Build();

         return host.RunAsync();
      }

      private static IAzure GetAzureInstance(HostBuilderContext hostContext)
      {
         string tenantId = Environment.GetEnvironmentVariable("AzSpTenantId");

         AzureCredentials credentials;
         if (hostContext.HostingEnvironment.IsProduction())
         {
            credentials = SdkContext.AzureCredentialsFactory
               .FromSystemAssignedManagedServiceIdentity(MSIResourceType.AppService, AzureEnvironment.AzureGlobalCloud, tenantId);
         }
         else
         {
            // authenticate with Service Principal credentials
            string clientId = Environment.GetEnvironmentVariable("AzSpClientId");
            string clientSecret = hostContext.Configuration["AzSpClientSecret"];
            credentials = SdkContext.AzureCredentialsFactory
               .FromServicePrincipal(clientId, clientSecret, tenantId, environment: AzureEnvironment.AzureGlobalCloud);
         }

         return Microsoft.Azure.Management.Fluent.Azure
            .Authenticate(credentials)
            .WithDefaultSubscription();
      }

      private static HttpClient ConfigureAzureRestApiClient(HttpClient client)
      {
         // TODO: configure for Azure auth
         client.DefaultRequestHeaders.Add("TODO", "TODO");
         return client;
      }
   }
}
