@description('The name of the SQL logical server.')
param serverName string = ''

@description('The name of the SQL Database.')
param sqlDBName string = 'DeleteMeIfDeployed'

@description('The administrator username of the SQL logical server.')
param administratorLogin string = ''

@description('The administrator password of the SQL logical server.')
param administratorLoginEngima string = ''

param elasticJobAgentName string = ''

param location string

var createSingleDatabase = (sqlDBName != 'DeleteMeIfDeployed')

resource SqlServer 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginEngima
    version: '12.0'
  }
}

resource SqlServer_SqlDBName 'Microsoft.Sql/servers/databases@2020-08-01-preview' = if (createSingleDatabase) {
  name: '${SqlServer.name}/${sqlDBName}'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Standard'
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

resource elasticJobAgent 'Microsoft.Sql/servers/jobAgents@2021-02-01-preview' = {
  name: '${SqlServer.name}/${elasticJobAgentName}'
  location: 'westus'
  sku: {
    name: 'Agent'
    capacity: 100
  }
  properties: {
    databaseId: SqlServer_SqlDBName.id
  }
}

output sqlServerName string = SqlServer.name
