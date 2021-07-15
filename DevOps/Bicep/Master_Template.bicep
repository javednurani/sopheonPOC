@description('The name of the Storage Account')
param storageAccounts_name string = 'stratuswebsiteneptune'

@description('The name of the Key Vault')
param keyVault_name string = 'Stratus-Neptune'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// Storage Account module
module SpaStorageAccount 'Storage_Account.bicep' = {
  name: 'SpaStorageAccount'
  params: {
    location: location
    storageAccounts_name: storageAccounts_name
  }
}

// Key Vault module
module KeyVault 'Key_Vault.bicep' = {
  name: 'KeyVault'
  params: {
    location: location
    keyVault_name: keyVault_name
  }
}


