#define Managed
using System;
using System.Threading.Tasks;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using FluentValidation;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sopheon.CloudNative.Environments.Data;
using Sopheon.CloudNative.Environments.Domain.Repositories;
using Sopheon.CloudNative.Environments.Functions.Helpers;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;

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
            })
            // Cloud-1484, we are defining ObjectSerializer to be used, per Function class
            // this is due to unit test context not having a serializer configured, if we use the below line to configure serializer for production context
            //.ConfigureFunctionsWorkerDefaults(worker => worker.UseNewtonsoftJson())
            .ConfigureFunctionsWorkerDefaults()
            .ConfigureOpenApi()
            .ConfigureServices((hostContext, services) =>
            {
               // Add Logging
               services.AddLogging();

               // Add HttpClient
               services.AddHttpClient();

                // Add Custom Services
                string connString = Environment.GetEnvironmentVariable("SQLCONNSTR_EnvironmentsSqlConnectionString");
                services.AddDbContext<EnvironmentContext>(options => options.UseSqlServer(connString));
               services.AddAutoMapper(typeof(Program));

               services.AddScoped<IEnvironmentRepository, EFEnvironmentRepository>();
               services.AddScoped<IValidator<EnvironmentDto>, EnvironmentDtoValidator>();
               services.AddScoped<HttpResponseDataBuilder>();
            })
            .Build();

         return host.RunAsync();
      }
   }
}
