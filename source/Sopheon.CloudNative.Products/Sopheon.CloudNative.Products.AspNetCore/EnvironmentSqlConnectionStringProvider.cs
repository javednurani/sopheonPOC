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
      private readonly IConfiguration _configRoot;

      public EnvironmentSqlConnectionStringProvider(IHttpContextAccessor accessor,
         IEnvironmentIdentificationService tenantEnvironmentIdentificationService,
         IHttpClientFactory clientFactory,
         IMemoryCache memoryCache,
         IConfiguration configRoot)
      {
         _httpContext = accessor.HttpContext;
         _tenantEnvironmentIdentificationService = tenantEnvironmentIdentificationService;
         _clientFactory = clientFactory;
         _memoryCache = memoryCache;
         _configRoot = configRoot;
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
         string apiUrl = _configRoot.GetValue<string>("ServiceUrls:GetEnvironmentResourceBindingUri"); 
         string businessServiceName = "ProductManagement";
         string dependencyName = "ProductManagementSqlDb";
         string requestUrl = $"{apiUrl}({environmentKey}, {businessServiceName}, {dependencyName})";

         string connectionString = await CallCatalogService(requestUrl); // TODO: Retry and Backoff Logic
         return connectionString;
      }

      private async Task<string> CallCatalogService(string requestUrl)
      {
         var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
         request.Headers.Add("User-Agent", "Sopheon.CloudNative.Products.AspNetCore");

         var client = _clientFactory.CreateClient("EnvFunction");

         
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
}
