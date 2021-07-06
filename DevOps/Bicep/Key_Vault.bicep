@description('The name of the Key Vault')
param keyVault_name string = '&KeyVaultName&'

param location string

resource KeyVaultAccount 'Microsoft.KeyVault/vaults@2021-04-01-preview' = {
  name: keyVault_name
  location: location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: subscription().tenantId
    accessPolicies: [ 
      {
        tenantId: subscription().tenantId
        objectId: reference(resourceId('Microsoft.KeyVault/vaults', 'Stratus-Dev'), '2021-04-01-preview', 'Full').identity.principalId
        permissions: {
          keys: [
            'get'
            'list'
          ]
          secrets: [
            'get'
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

resource KeyVaultSecret_StratusB2CClientId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CClientId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CExtensionsClientId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CExtensionsClientId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CExtensionsObjectId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CExtensionsObjectId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CIdentityFrameworkClientId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CIdentityFrameworkClientId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CJWTClientId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CJWTClientId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CProxyIdentityFrameworkClientId 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CProxyIdentityFrameworkClientId'
  properties: {
    attributes: {
      enabled: true
    }
  }
}

resource KeyVaultSecret_StratusB2CTenantName 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault_name}/StratusB2CTenantName'
  properties: {
    attributes: {
      enabled: true
    }
  }
}
