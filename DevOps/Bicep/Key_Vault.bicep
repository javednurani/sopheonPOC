@description('The name of the Key Vault')
param keyVault_name string = '^KeyVaultName^'

param location string

resource KeyVaultAccount 'Microsoft.KeyVault/vaults@2021-04-01-preview' = {
  name: keyVault_name
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    enableSoftDelete: false
    tenantId: subscription().tenantId
    accessPolicies: [ 
      {
        tenantId: subscription().tenantId
        objectId: 'b48ecddf-54ee-499a-b97f-4a2236881039' //'reference(resourceId('Stratus-Dev', 'Microsoft.KeyVault/vaults', 'Stratus-Dev'), '2021-04-01-preview', 'Full').identity.principalId
        permissions: {
          keys: [
            'get'
            'list'
          ]
          secrets: [
            'get'
            'set'
            'list'
          ]
          certificates: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}
