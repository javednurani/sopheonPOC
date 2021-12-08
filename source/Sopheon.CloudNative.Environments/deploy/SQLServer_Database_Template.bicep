@description('The name of the SQL logical server.')
param serverName string = ''

@description('The name of the SQL Database.')
param sqlDBName string = ''

@description('The administrator username of the SQL logical server.')
param administratorLogin string = ''

@description('The administrator password of the SQL logical server.')
param administratorLoginEngima string = ''

param location string

resource SqlServer 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginEngima
    version: '12.0'
  }
}

resource SqlServer_SqlDBName 'Microsoft.Sql/servers/databases@2020-08-01-preview' = if (!empty(sqlDBName)) {
  name: '${SqlServer.name}/${sqlDBName}'
  location: location
  sku: {
    name: 'Basic'
    tier: 'Basic'
    capacity: 0
  }
}

resource SqlServer_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2020-02-02-preview' = {
  name: '${SqlServer.name}/AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
