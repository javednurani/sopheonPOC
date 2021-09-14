#define Managed
using System;
using System.Threading.Tasks;
using Azure.Core.Serialization;
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
                  connString = hostContext.Configuration["SQLCONNSTR_EnvironmentsSqlConnectionString"];
               }
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
