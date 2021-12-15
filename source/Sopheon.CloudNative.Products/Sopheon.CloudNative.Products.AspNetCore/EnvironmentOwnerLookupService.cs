using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Products.Utilities;

namespace Sopheon.CloudNative.Products.AspNetCore
{
   public class EnvironmentOwnerLookupService
   {
      private readonly HttpContext _httpContext;
      private readonly IHttpClientFactory _clientFactory;
      private readonly IMemoryCache _memoryCache;
      private readonly IConfiguration _configRoot;
      private readonly ILogger<EnvironmentOwnerLookupService> _logger;

      public EnvironmentOwnerLookupService(IHttpContextAccessor accessor,
         IHttpClientFactory clientFactory,
         IMemoryCache memoryCache,
         IConfiguration configRoot,
         ILogger<EnvironmentOwnerLookupService> logger)
      {
         _httpContext = accessor.HttpContext;
         _clientFactory = clientFactory;
         _memoryCache = memoryCache;
         _configRoot = configRoot;
         _logger = logger;
      }

      public async Task<string> GetEnvironmentOwnerAsync(string environmentKey) 
      {
         string cacheKey = $"{nameof(EnvironmentOwnerLookupService)}.{nameof(GetEnvironmentOwnerAsync)}:{environmentKey}";

         if (!_memoryCache.TryGetValue<string>(cacheKey, out string environmentOwner)) 
         {
            List<EnvironmentCatalogEntry> environments = await GetEnvironments();
            environmentOwner = environments?.FirstOrDefault(e => e.EnvironmentKey.Equals(environmentKey, StringComparison.OrdinalIgnoreCase))?.Owner;

            if (!string.IsNullOrWhiteSpace(environmentOwner)) 
            {
               _memoryCache.Set(cacheKey, environmentOwner, DateTime.UtcNow.AddMinutes(10));
            }
         }

         return environmentOwner;
      }

      private async Task<List<EnvironmentCatalogEntry>> GetEnvironments()
      {
         string requestUrl = _configRoot.GetValue<string>("ServiceUrls:GetEnvironments");

         List<EnvironmentCatalogEntry> environments = await CallCatalogService(requestUrl); // TODO: Retry and Backoff Logic
         return environments;
      }

      private async Task<List<EnvironmentCatalogEntry>> CallCatalogService(string requestUrl)
      {
         var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
         request.Headers.Add("User-Agent", "Sopheon.CloudNative.Products.AspNetCore");

         var client = _clientFactory.CreateClient("EnvFunction");

         List<EnvironmentCatalogEntry> environments = null;
         try
         {
            var response = await client.SendAsync(request);

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
               environments = JsonSerializer.Deserialize<List<EnvironmentCatalogEntry>>(responseString); // TODO: Create Client Library for Function Endpoint
            }
            else 
            {
               _logger.LogError($"Error calling {requestUrl}");
            }
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, $"Error calling {requestUrl}");
         }

         return environments;
      }

      private class EnvironmentCatalogEntry
      {
         public string EnvironmentKey { get; set; }

         public string Owner { get; set; }
      }
   }
}
