@description('The name of the Storage Account')
param storageAccounts_name string = '&StorageAccountName&'

param location string

resource StaticWebpage_StorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccounts_name
  location: location
  sku: {
    name: 'Standard_LRS'
    tier: 'Standard'
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
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

resource StaticWebpage_Storage_BlobService 'Microsoft.Storage/storageAccounts/blobServices@2021-04-01' = {
  name: '${StaticWebpage_StorageAccount.name}/default'
  properties: {
    deleteRetentionPolicy: {
      enabled: false
    }
  }
}

resource StaticWebpage_Storage_FileService 'Microsoft.Storage/storageAccounts/fileServices@2021-04-01' = {
  name: '${StaticWebpage_StorageAccount.name}/default'
}

resource StaticWebpage_Storage_QueueService 'Microsoft.Storage/storageAccounts/queueServices@2021-04-01' = {
  name: '${StaticWebpage_StorageAccount.name}/default'
}

resource StaticWebpage_Storage_TableService 'Microsoft.Storage/storageAccounts/tableServices@2021-04-01' = {
  name: '${StaticWebpage_StorageAccount.name}/default'
}

resource StaticWebpage_Storage_BlobService_WebContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${StaticWebpage_Storage_BlobService.name}/$web'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [
    StaticWebpage_StorageAccount
  ]
}
