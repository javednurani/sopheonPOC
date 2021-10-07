@description('The name of the SQL logical server.')
param serverName string = '^SqlServerName^'

@description('The name of the SQL Elastic Pool')
param poolName string = '^SqlElasticPoolName^'

@description('The administrator username of the SQL logical server.')
param administratorLogin string = 'sopheon'

@description('The administrator password of the SQL logical server.')
param administratorLoginEngima string = '^SqlAdminEngima^'

param location string = resourceGroup().location

var bufferCapacity = 100

resource SqlServer 'Microsoft.Sql/servers@2020-02-02-preview' = {
  name: serverName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorLoginEngima
    version: '12.0'
  }
}

resource SqlServer_Pool 'Microsoft.Sql/servers/elasticPools@2020-08-01-preview' = {
  name: '${SqlServer.name}/${poolName}'
  location: location
  sku: {
    name: 'BasicPool'
    tier: 'Basic'
    capacity: bufferCapacity
  }
  properties: {
    maxSizeBytes: 5242880000
    perDatabaseSettings: {
      minCapacity: 0
      maxCapacity: 5
    }
    zoneRedundant: false
  }
}

resource SqlServer_SqlDBName 'Microsoft.Sql/servers/databases@2020-08-01-preview' = [for i in range(0, bufferCapacity): {
  name: '${SqlServer.name}/${uniqueString(resourceGroup().id, SqlServer_Pool.name)}-${i}'
  location: location
  sku: {
    name: 'ElasticPool'
    tier: 'Basic'
    capacity: 0
  }
  properties: {
    elasticPoolId: SqlServer_Pool.id
  }
}]

resource SqlServer_AllowAllWindowsAzureIps 'Microsoft.Sql/servers/firewallrules@2020-02-02-preview' = {
  name: '${SqlServer.name}/AllowAllWindowsAzureIps'
  properties: {
    endIpAddress: '0.0.0.0'
    startIpAddress: '0.0.0.0'
  }
}
