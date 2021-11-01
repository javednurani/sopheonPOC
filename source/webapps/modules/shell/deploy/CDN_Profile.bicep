@description('The name of the CDN Profile')
param profiles_cdn_name string = '^CDNProfileName^'

@description('The name of the CDN Endpoint')
param profile_endpoint_name string = '^CDNProfileEndpointName^'

@description('The Storage Account Endpoint Origin')
param profile_endpoint_origin string = '^CDNProfileEndpointOrigin^'

@description('The name of the Marketing CDN Endpoint')
param profile_endpoint_marketing_name string = '^CDNProfileEndpointMarketingName^'

resource CDN_Profile 'Microsoft.Cdn/profiles@2020-09-01' = {
  name: profiles_cdn_name
  location: 'Global' 
  sku: {
    name: 'Standard_Microsoft'
  }
}

resource CDN_Profile_Endpoint 'Microsoft.Cdn/profiles/endpoints@2020-09-01' = {
  name: '${CDN_Profile.name}/${profile_endpoint_name}'
  location: 'Global'
  properties: {
    originHostHeader: profile_endpoint_origin
    contentTypesToCompress: [
      'application/eot'
      'application/font'
      'application/font-sfnt'
      'application/javascript'
      'application/json'
      'application/opentype'
      'application/otf'
      'application/pkcs7-mime'
      'application/truetype'
      'application/ttf'
      'application/vnd.ms-fontobject'
      'application/xhtml+xml'
      'application/xml'
      'application/xml+rss'
      'application/x-font-opentype'
      'application/x-font-truetype'
      'application/x-font-ttf'
      'application/x-httpd-cgi'
      'application/x-javascript'
      'application/x-mpegurl'
      'application/x-opentype'
      'application/x-otf'
      'application/x-perl'
      'application/x-ttf'
      'font/eot'
      'font/ttf'
      'font/otf'
      'font/opentype'
      'image/svg+xml'
      'text/css'
      'text/csv'
      'text/html'
      'text/javascript'
      'text/js'
      'text/plain'
      'text/richtext'
      'text/tab-separated-values'
      'text/xml'
      'text/x-script'
      'text/x-component'
      'text/x-java-source'
    ]
    isCompressionEnabled: true
    isHttpAllowed: false
    isHttpsAllowed: true
    queryStringCachingBehavior: 'IgnoreQueryString'
    optimizationType: 'GeneralWebDelivery'
    origins: [
      {
        name: profile_endpoint_name
        properties: {
          hostName: profile_endpoint_origin
          originHostHeader: profile_endpoint_origin
          priority: 1
          weight: 1000
          enabled: true
        }
      }
    ]
    deliveryPolicy: {
      rules: [
        {
          name: 'Global'
          order: 0
          actions: [
            {
              name: 'CacheExpiration'
              parameters: {
                cacheBehavior: 'BypassCache'
                cacheType: 'All'
                '@odata.type': '#Microsoft.Azure.Cdn.Models.DeliveryRuleCacheExpirationActionParameters'
              }
            }
          ]
        }
        {
          name: 'ReactRouting'
          order: 1
          conditions: [
            {
              name: 'UrlFileExtension'
              parameters: {
                operator: 'GreaterThan'
                negateCondition: true
                matchValues: [
                  '0'
                ]
                '@odata.type': '#Microsoft.Azure.Cdn.Models.DeliveryRuleUrlFileExtensionMatchConditionParameters'
              }
            }
          ]
          actions: [
            {
              name: 'UrlRewrite'
              parameters: {
                sourcePattern: '/'
                destination: '/index.html'
                preserveUnmatchedPath: false
                '@odata.type': '#Microsoft.Azure.Cdn.Models.DeliveryRuleUrlRewriteActionParameters'
              }
            }
          ]
        }
      ]
    }
  }
}

resource CDN_Profile_Marketing_Endpoint 'Microsoft.Cdn/profiles/endpoints@2020-09-01' = {
  name: '${CDN_Profile.name}/${profile_endpoint_marketing_name}'
  location: 'Global'
  properties: {
    originHostHeader: profile_endpoint_origin
    originPath: 'Marketing/'
    contentTypesToCompress: [
      'application/eot'
      'application/font'
      'application/font-sfnt'
      'application/javascript'
      'application/json'
      'application/opentype'
      'application/otf'
      'application/pkcs7-mime'
      'application/truetype'
      'application/ttf'
      'application/vnd.ms-fontobject'
      'application/xhtml+xml'
      'application/xml'
      'application/xml+rss'
      'application/x-font-opentype'
      'application/x-font-truetype'
      'application/x-font-ttf'
      'application/x-httpd-cgi'
      'application/x-javascript'
      'application/x-mpegurl'
      'application/x-opentype'
      'application/x-otf'
      'application/x-perl'
      'application/x-ttf'
      'font/eot'
      'font/ttf'
      'font/otf'
      'font/opentype'
      'image/svg+xml'
      'text/css'
      'text/csv'
      'text/html'
      'text/javascript'
      'text/js'
      'text/plain'
      'text/richtext'
      'text/tab-separated-values'
      'text/xml'
      'text/x-script'
      'text/x-component'
      'text/x-java-source'
    ]
    isCompressionEnabled: true
    isHttpAllowed: false
    isHttpsAllowed: true
    queryStringCachingBehavior: 'IgnoreQueryString'
    optimizationType: 'GeneralWebDelivery'
    origins: [
      {
        name: profile_endpoint_name
        properties: {
          hostName: profile_endpoint_origin
          originHostHeader: profile_endpoint_origin
          priority: 1
          weight: 1000
          enabled: true
        }
      }
    ]
  }
}
