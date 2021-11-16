
param location string = resourceGroup().location

@description('Name of the Azure Function App')
param functionAppName string = '^EnvironmentsFunctionAppName^'

param appInsightsName string = '^AppInsightsName^'

param storageAccountName string = '^EnvironmentFunctionStorageAccountName^'

param webServerFarm_Name string = '^WebServerFarmName^'

param sqlServer_Name string = '^SqlServerName^'

var functionRuntime = 'dotnet-isolated'

var keyVaultName = resourceGroup().name

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

resource EnvironmentFunctionApp_Storage_BlobService 'Microsoft.Storage/storageAccounts/blobServices@2021-04-01' = {
  name: '${storageAccount.name}/default'
  properties: {
    deleteRetentionPolicy: {
      enabled: false
    }
  }
}

resource StaticWebpage_Storage_BlobService_ArmTemplateContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${EnvironmentFunctionApp_Storage_BlobService.name}/armtemplates'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'Blob'
  }
  dependsOn: [
    storageAccount
  ]
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource WebServerFarmForFunction 'Microsoft.Web/serverfarms@2021-01-15' = {
  location: location
  name: webServerFarm_Name
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
  kind: 'functionapp'
}

resource EnvironmentsFunctionApp 'Microsoft.Web/sites@2021-01-15' = {
  location: location
  name: functionAppName
  kind: 'functionapp'
  properties: {
    serverFarmId: WebServerFarmForFunction.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: 'InstrumentationKey=${appInsights.properties.InstrumentationKey}'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionRuntime
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~3'
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
          name: 'AzSpSubscriptionId'
          value: subscription().subscriptionId
        }
        {
          name: 'AzResourceGroupName'
          value: resourceGroup().name
        }
        {
          name: 'AzSqlServerName'
          value: sqlServer_Name
        }
      ]
    }
  }
}

