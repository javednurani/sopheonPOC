
param location string = resourceGroup().location

@description('Name of the Azure Function App')
param functionAppName string = '^FunctionAppName^'

param appInsightsName string = '^AppInsightsName^'

param storageAccountName string = 'doesnotexist'

param webServerFarmName string = '^WebServerFarmName^'

param sqlServerName string = '^SqlServerName^'

param tenantEnvironmentServer string = '^AzTenantEnvironmentServer^'

param functionDotNetRuntime string = 'dotnet-isolated'

var keyVaultName = resourceGroup().name

resource FunctionsStorageAcccount 'Microsoft.Storage/storageAccounts@2021-06-01' existing = {
  name: storageAccountName
}

resource AppInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: appInsightsName
}

resource WebServerFarmForFunction 'Microsoft.Web/serverfarms@2021-01-15' = {
  location: location
  name: webServerFarmName
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  kind: 'functionapp'
}

resource FunctionApp 'Microsoft.Web/sites@2021-01-15' = {
  location: location
  name: functionAppName
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {    
    serverFarmId: WebServerFarmForFunction.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${FunctionsStorageAcccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(FunctionsStorageAcccount.id, FunctionsStorageAcccount.apiVersion).keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${FunctionsStorageAcccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(FunctionsStorageAcccount.id, FunctionsStorageAcccount.apiVersion).keys[0].value}'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: AppInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${AppInsights.properties.InstrumentationKey}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionDotNetRuntime
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'DatabaseBufferCapacity'
          value: '^DatabaseBufferCapacity^'
        }
        {
          name: 'DatabaseBufferTimer'
          value: '^DatabaseBufferTimerCron^'
        }
        {
          name: 'KeyVaultName'
          value: keyVaultName
        }
        {
          name: 'AzSpTenantId'
          value: tenant().tenantId
        }
        {
          name: 'AzSubscriptionId'
          value: subscription().subscriptionId
        }
        {
          name: 'AzResourceGroupName'
          value: resourceGroup().name
        }
        {
          name: 'AzSqlServerName'
          value: sqlServerName
        }
        {
          name: 'AzTenantEnvironmentServer'
          value: tenantEnvironmentServer
        }
      ]
    }
  }
}

output functionIdentity string = FunctionApp.identity.principalId
