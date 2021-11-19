@description('The name of the Storage Account')
param storageAccounts_name string = '^StorageAccountName^'

@description('B2C Login url')
param b2cLogin string = '^B2CLogin^'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// Storage Account module
module SpaStorageAccount 'Storage_Account.bicep' = {
  name: 'B2CAssets-Storage'
  params: {
    location: location
    storageAccounts_name: storageAccounts_name
    b2cLogin: b2cLogin
  }
}


