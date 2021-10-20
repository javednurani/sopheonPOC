@description('The name of the Environment Function App')
param environmentFunctionApp_Name string = '^EnvironmentFunctionAppName^'

param environmentFunctionAppStorage_name string = '^EnvironmentFunctionStorageAccountName^'

param appInsightsName string = '^AppInsightsName^'

@description('The name of the SQL Server')
param sqlServer_name string = '^SqlServerName^'

@description('Sql server pool name')
param sqlServerPool_name string = '^SqlElasticPoolName^'

@description('Sql server database name')
param sqlServerDatabase_name string = '^SqlServerDatabaseName^'

@description('Name of the WebServer Farm being used')
param webServerFarm_Name string = '^WebServerFarmName^'

@description('Sql Server Password')
param sqlServer_Enigma string = '^SqlAdminEngima^'

@description('Sql server admin')
param administratorLogin string = 'sopheon'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// SQL Server module
module SqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    serverName: sqlServer_name
    poolName:sqlServerPool_name
    sqlDBName:sqlServerDatabase_name
    administratorLogin: administratorLogin
  }
}

module EnvironmentFunction 'Environments_Function_App.bicep' = {
  name: 'Function-App-Deployment'
  params: {
    location: location
    storageAccountName: environmentFunctionAppStorage_name
    appInsightsName: appInsightsName
    functionAppName: environmentFunctionApp_Name
    webServerFarm_Name: webServerFarm_Name
    sqlServer_Name: sqlServer_name
  }
}

