@description('The name of the Storage Account')
param storageAccounts_name string = '^StorageAccountName^'

param location string

resource StaticWebpage_StorageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccounts_name
  location: location
  sku: {
    name: 'Standard_LRS'
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
    cors: {
      corsRules: [
        {
            allowedOrigins: [
                'https://stratusb2cdev.b2clogin.com'
            ]
            allowedMethods: [
                'GET'
                'OPTIONS'
            ]
            maxAgeInSeconds: 200
            exposedHeaders: [
                '*'
            ]
            allowedHeaders: [
                '*'
            ]
        }
      ]
    }
  }
}

resource StaticWebpage_Storage_BlobService_webContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${StaticWebpage_Storage_BlobService.name}/$web'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
  }
  dependsOn: [
    StaticWebpage_StorageAccount
  ]
}

resource StaticWebpage_Storage_BlobService_b2cassetsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${StaticWebpage_Storage_BlobService.name}/b2cassets'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'Blob'
  }
  dependsOn: [
    StaticWebpage_StorageAccount
  ]
}

