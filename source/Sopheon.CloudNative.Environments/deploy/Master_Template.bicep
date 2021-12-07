@description('The name of the Environment Function App')
param environmentFunctionApp_Name string = '^EnvironmentFunctionAppName^'

@description('The name of the Environment Management Function App Storage Account')
param environmentFunctionAppStorage_name string = '^EnvironmentFunctionStorageAccountName^'

@description('The name of the AppInsights instance')
param appInsightsName string = '^AppInsightsName^'

@description('The name of the Environment Management SQL server')
param envManagement_sqlServer_name string = '^EnvManagementSqlServerName^'

@description('The name of the Elastic Job Agent SQL server')
param elasticJobAgent_sqlServer_name string = '^ElasticJobAgentSqlServerName^'

@description('The name fo the Tenant SQL server')
param tenant_sqlServer_name string = '^TenantSqlServerName^'

@description('Environment Management SQL server database name')
param envManagement_sqlServerDatabase_name string = '^EnvironmentManagementSqlServerDatabaseName^'

@description('Elastic Job Agent SQL server database name')
param elasticJobAgent_sqlServerDatabase_name string = '^ElasticJobAgentSqlServerDatabaseName^'

@description('Name of the WebServer Farm being used')
param webServerFarm_Name string = '^WebServerFarmName^'

@description('Sql server password')
param sqlServer_Enigma string = '^SqlAdminEngima^'

@description('Sql server admin')
param administratorLogin string = 'sopheon'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// Environment Management SQL Server module
module EnvironmentManagementSqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'EnvironmentMangement-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    serverName: envManagement_sqlServer_name
    useElasticPool: false
    sqlDBName: envManagement_sqlServerDatabase_name
    administratorLogin: administratorLogin
  }
}

// Elastic Job Agent SQL Server module
module ElasticJobAgentSqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'ElasticJobAgent-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    useElasticPool: false
    serverName: elasticJobAgent_sqlServer_name
    sqlDBName: elasticJobAgent_sqlServerDatabase_name
    administratorLogin: administratorLogin
  }
}

// Tenant SQL Server module
module TenantSqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'Tenant-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    serverName: tenant_sqlServer_name
    useElasticPool: false
    sqlDBName: tenant_sqlServerDatabase_name
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
    sqlServer_Name: toLower(EnvironmentManagementSqlServer.name)
  }
}

