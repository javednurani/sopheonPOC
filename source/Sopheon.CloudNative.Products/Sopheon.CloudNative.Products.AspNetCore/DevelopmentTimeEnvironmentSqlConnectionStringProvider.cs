using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public class DevelopmentTimeEnvironmentSqlConnectionStringProvider : IEnvironmentSqlConnectionStringProvider
   {
      private readonly HttpContext _httpContext;
      private readonly IEnvironmentIdentificationService _tenantEnvironmentIdentificationService;
      private readonly IConfiguration _configuration;

      public DevelopmentTimeEnvironmentSqlConnectionStringProvider(IHttpContextAccessor accessor,
         IEnvironmentIdentificationService tenantEnvironmentIdentificationService,
         IConfiguration configuration)
      {
         _httpContext = accessor.HttpContext;
         _tenantEnvironmentIdentificationService = tenantEnvironmentIdentificationService;
         _configuration = configuration;
      }

      public Task<string> GetConnectionStringAsync()
      {
         Dictionary<string, string> environmentDatabaseCatalog = _configuration.GetSection("DevelopmentAndDemoSettings:TenantEnvironmentDatabases").Get<Dictionary<string, string>>();

         environmentDatabaseCatalog = new Dictionary<string, string>(environmentDatabaseCatalog, StringComparer.OrdinalIgnoreCase);

         string result = environmentDatabaseCatalog.TryGetValue(_tenantEnvironmentIdentificationService.GetEnvironmentIdentifier(_httpContext), out string connectionString) ? connectionString : null;

         return Task.FromResult(result);
      }
   }
}
