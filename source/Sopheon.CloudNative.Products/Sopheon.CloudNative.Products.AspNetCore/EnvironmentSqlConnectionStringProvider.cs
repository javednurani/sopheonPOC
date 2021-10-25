using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Sopheon.CloudNative.Products.Utilities;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public class EnvironmentSqlConnectionStringProvider : IEnvironmentSqlConnectionStringProvider
   {
      private readonly HttpContext _httpContext;
      private readonly IEnvironmentIdentificationService _tenantEnvironmentIdentificationService;
      private readonly IHttpClientFactory _clientFactory;
      private readonly IMemoryCache _memoryCache;

      public EnvironmentSqlConnectionStringProvider(IHttpContextAccessor accessor,
         IEnvironmentIdentificationService tenantEnvironmentIdentificationService,
         IHttpClientFactory clientFactory,
         IMemoryCache memoryCache)
      {
         _httpContext = accessor.HttpContext;
         _tenantEnvironmentIdentificationService = tenantEnvironmentIdentificationService;
         _clientFactory = clientFactory;
         _memoryCache = memoryCache;
      }

      public async Task<string> GetConnectionStringAsync()
      {
         string environmentKey = _tenantEnvironmentIdentificationService.GetEnvironmentIdentifier(_httpContext);

         string cacheKey = $"{nameof(EnvironmentSqlConnectionStringProvider)}.{nameof(GetConnectionStringAsync)}:{environmentKey}";

         if (!_memoryCache.TryGetValue(cacheKey, out string connectionString))
         {
            connectionString = await LookupConnectionString(environmentKey);
            _memoryCache.Set(cacheKey, connectionString, DateTimeOffset.Now.AddMinutes(10));
         }

         return connectionString;
      }

      private async Task<string> LookupConnectionString(string environmentKey)
      {
         // TODO: Call Environment Lookup Azure Function: https://stratus-qa.azurewebsites.net​/GetEnvironmentResourceBindingUri({environmentKey},{businessServiceName},{dependencyName})

         string apiUrl = "http://localhost:7071/GetEnvironmentResourceBindingUri";
         string businessServiceName = "ProductManagement";
         string dependencyName = "SqlDatabase";
         //http://localhost:7071/GetEnvironmentResourceBindingUri(34117853-734d-44d3-aec7-9267e9e5e545,ProductManagement,SqlDatabase)
         //http://localhost:7071/GetEnvironmentResourceBindingUri(34117853-734d-44d3-aec7-9267e9e5e545,%20ProductManagement,%20SqlDatabase)
         string requestUrl = $"{apiUrl}({environmentKey}, {businessServiceName}, {dependencyName})";
         string connectionString = await CallCatalogService(requestUrl); // TODO: Retry and Backoff Logic
         return connectionString;
      }

      private async Task<string> CallCatalogService(string requestUrl)
      {
         var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
         //request.Headers.Add("Accept", "application/vnd.github.v3+json");
         request.Headers.Add("User-Agent", "Sopheon.CloudNative.Products.AspNetCore");

         var client = _clientFactory.CreateClient();

         var response = await client.SendAsync(request);

         string connectionString = null;
         if (response.IsSuccessStatusCode)
         {
            string responseString = null;
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            {
               using (StreamReader streamReader = new StreamReader(responseStream))
               {
                  responseString = streamReader.ReadToEnd();
               }
            }
            connectionString = JsonSerializerExtensions.DeserializeAnonymousType(responseString, new { Uri = "" }).Uri; // TODO: Create Client Library for Function Endpoint
         }

         return connectionString;
      }
   }

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
         Dictionary<string, string> environmentDatabaseCatalog = _configuration.GetSection("LocalDevelopment:TenantEnvironmentDatabases").Get<Dictionary<string, string>>();

         environmentDatabaseCatalog = new Dictionary<string, string>(environmentDatabaseCatalog, StringComparer.OrdinalIgnoreCase);

         string result = environmentDatabaseCatalog.TryGetValue(_tenantEnvironmentIdentificationService.GetEnvironmentIdentifier(_httpContext), out string connectionString) ? connectionString : null;

         return Task.FromResult(result);
      }
   }
}
