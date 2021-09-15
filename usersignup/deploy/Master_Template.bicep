@description('The name of the Storage Account')
param storageAccounts_name string = '^StorageAccountName^'

param location string = resourceGroup().location

// Storage Account module
module SpaStorageAccount 'Storage_Account.bicep' = {
  name: 'Website-Storage'
  params: {
    location: location
    storageAccounts_name: storageAccounts_name
  }
}


