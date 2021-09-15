@description('The name of the Storage Account')
param storageAccounts_name string = '^StorageAccountName^'

// Storage Account module
module SpaStorageAccount 'Storage_Account.bicep' = {
  name: 'Website-Storage'
  params: {
    storageAccounts_name: storageAccounts_name
  }
}


