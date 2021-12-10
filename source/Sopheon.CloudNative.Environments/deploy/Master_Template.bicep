@description('The name of the Environment Function App')
param environmentFunctionApp_Name string = '^EnvironmentFunctionAppName^'

@description('The name of the Environment Management Function App Storage Account')
param environmentFunctionAppStorage_name string = '^EnvironmentFunctionStorageAccountName^'

@description('The name of the AppInsights instance')
param appInsightsName string = '^AppInsightsName^'

@description('The name of the Environment Management SQL server')
param environmentManagement_sqlServer_name string = '^EnvironmentManagementSQLServerName^'

@description('The name of the Elastic Job Agent SQL server')
param elasticJobAgent_sqlServer_name string = '^ElasticJobAgentSQLServerName^'

@description('The name fo the Tenant SQL server')
param tenant_sqlServer_name string = '^TenantSQLServerName^'

@description('Environment Management SQL server database name')
param envManagement_sqlServerDatabase_name string = '^EnvironmentManagementSQLServerDatabaseName^'

@description('Elastic Job Agent SQL server database name')
param elasticJobAgent_sqlServerDatabase_name string = '^ElasticJobAgentSQLServerDatabaseName^'

@description('Name of the WebServer Farm being used')
param webServerFarm_Name string = '^WebServerFarmName^'

@description('Sql server password')
param sqlServer_Enigma string = '^SqlAdminEngima^'

@description('Sql server admin')
param administratorLogin string = 'sopheon'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

@description('Name of the WebServer Farm being used')
param webServerFarm_Named string = '^WebServerFarmNamed^'

@description('The name of the Environment Function App')
param environmentFunctionApp_Named string = '^EnvironmentFunctionAppNamed^'

@description('The name of the Environment Management Function App Storage Account')
param environmentFunctionAppStorage_named string = '^EnvironmentFunctionStorageAccountNamed^'

@description('The name of the AppInsights instance')
param appInsightsNamed string = '^AppInsightsNamed^'

// Environment Management SQL Server module
module EnvironmentManagementSqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'EnvironmentMangement-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    serverName: environmentManagement_sqlServer_name
    sqlDBName: envManagement_sqlServerDatabase_name
    administratorLogin: administratorLogin
  }
}

// Create blank tenant database, when re-ran the system will see it already exists
module EnvironmentManagementSqlServerTenantTemplateDatabase 'SQLServer_Database_Template.bicep' = {
  name: 'EnvironmentMangemenTenantTemplateDatabase-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
    serverName: EnvironmentManagementSqlServer.outputs.sqlServerName
    sqlDBName: 'TenantEnvironmentTemplate' 
    administratorLogin: administratorLogin
  }
}

// Elastic Job Agent SQL Server module
module ElasticJobAgentSqlServer 'SQLServer_Database_Template.bicep' = {
  name: 'ElasticJobAgent-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
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

module EnvironmentFunctiond 'Environments_Function_App.bicep' = {
  name: 'Function-App-Deploymentd'
  params: {
    location: location
    storageAccountName: environmentFunctionAppStorage_named
    appInsightsName: appInsightsNamed
    functionAppName: environmentFunctionApp_Named
    webServerFarm_Name: webServerFarm_Named
    sqlServer_Name: toLower(EnvironmentManagementSqlServer.name)
  }
}
