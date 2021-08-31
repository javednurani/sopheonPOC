@description('The name of the Storage Account')
param storageAccounts_name string = '^StorageAccountName^'

@description('The name of the Key Vault')
param keyVault_name string = '^KeyVaultName^'

@description('The name of the SQL Server')
param sqlServer_name string = '^SQLServerName^'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// Storage Account module
module SpaStorageAccount 'Storage_Account.bicep' = {
  name: storageAccounts_name
  params: {
    location: location
  }
}

// Key Vault module
module KeyVault 'Key_Vault.bicep' = {
  name: keyVault_name
  params: {
    location: location
  }
}

module SqlServer 'SQLServer_Database_Template.bicep' = {
  name: sqlServer_name
  params: {
    location: location
    administratorLoginEngima: ''
  }
}

