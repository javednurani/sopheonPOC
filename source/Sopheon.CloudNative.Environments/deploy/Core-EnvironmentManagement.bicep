@description('The name of the Storage Account for the function apps')
@maxLength(24)
param functionAppStorageName string = '^FunctionStorageAccountName^'

param appInsightsName string = '^AppInsightsName^'

param location string = resourceGroup().location

resource StorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: functionAppStorageName
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
  name: '${StorageAccount.name}/default'
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
}

resource StaticWebpage_Storage_BlobService_EFMigrationsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${EnvironmentFunctionApp_Storage_BlobService.name}/efmigrations'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'Blob'
  }
}

resource AppInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

output storageAccountName string = StorageAccount.name
output appInsightsName string = AppInsights.name
