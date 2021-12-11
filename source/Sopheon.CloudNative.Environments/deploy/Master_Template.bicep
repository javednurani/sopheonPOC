// Core
@description('The name of the AppInsights instance')
param appInsightsName string = '^AppInsightsName^'

@description('The name of the Environment Management SQL server')
param environmentManagementSQLServerName string = '^EnvironmentManagementSQLServerName^'

@description('The name of the Elastic Job Agent SQL server')
param elasticJobAgentSQLServerName string = '^ElasticJobAgentSQLServerName^'

@description('The name fo the Tenant SQL server')
param tenantSQLServerName string = '^TenantSQLServerName^'

@description('Sql server password')
param sqlServerEnigma string = '^SqlAdminEngima^'

@description('Sql server admin')
param administratorLogin string = 'sopheon'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

@description('Elastic Job Agent SQL server database name')
param elasticJobAgentSQLServerDatabaseName string = '^ElasticJobAgentSQLServerDatabaseName^'

@description('Environment Management SQL server database name')
param envManagementSQLServerDatabaseName string = '^EnvironmentManagementSQLServerDatabaseName^'

@description('The name of the Storage Account for the function apps')
@maxLength(24)
param functionAppStorageName string = '^FunctionStorageAccountName^'


// Environment Management API
@description('The name of the Environment Function App')
param environmentFunctionAppName string = '^EnvironmentFunctionAppName^'

@description('Name of the WebServer Farm being used for Environment Management API')
param environmentWebServerFarmName string = '^EnvironmentWebServerFarmName^'


// Resource Management API
@description('The name of the Resource Management Function App')
param resourceManagementFunctionAppName string = '^ResourceManagementFunctionAppName^'

@description('Name of the WebServer Farm being used for Resource Management API')
param resourceWebServerFarmName string = '^ResourceWebServerFarmName^'

module CoreServices 'Core-EnvironmentManagement.bicep' = {
  name: 'Core-Services-Resources'
  params: {
    location: location
    functionAppStorageName: functionAppStorageName
    appInsightsName: appInsightsName
  }
}

// Environment Management SQL Server module
module EnvironmentManagementSqlServer 'SQLServer_SingleDatabase.bicep' = {
  name: 'EnvironmentMangement-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServerEnigma
    serverName: environmentManagementSQLServerName
    sqlDBName: envManagementSQLServerDatabaseName
    administratorLogin: administratorLogin
  }
}

// Create blank tenant database, when re-ran the system will see it already exists
module EnvironmentManagementSqlServerTenantTemplateDatabase 'SQLServer_SingleDatabase.bicep' = {
  name: 'EnvironmentMangemenTenantTemplateDatabase-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServerEnigma
    serverName: EnvironmentManagementSqlServer.outputs.sqlServerName
    sqlDBName: 'TenantEnvironmentTemplate' 
    administratorLogin: administratorLogin
  }
}

// Elastic Job Agent SQL Server module
module ElasticJobAgentSqlServer 'SQLServer_SingleDatabase.bicep' = {
  name: 'ElasticJobAgent-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServerEnigma
    serverName: elasticJobAgentSQLServerName
    sqlDBName: elasticJobAgentSQLServerDatabaseName
    administratorLogin: administratorLogin
  }
}

// Tenant SQL Server module
module TenantSqlServer 'SQLServer_SingleDatabase.bicep' = {
  name: 'Tenant-Sql-Server-Deployment'
  params: {
    location: location
    administratorLoginEngima: sqlServerEnigma
    serverName: tenantSQLServerName
    administratorLogin: administratorLogin
  }
}

module EnvironmentFunction 'Function_App.bicep' = {
  name: 'Environment-Function-App-Deployment'
  params: {
    location: location
    storageAccountName: CoreServices.outputs.storageAccountName
    appInsightsName: CoreServices.outputs.appInsightsName
    functionDotNetRuntime: 'dotnet-isolated'
    tenantEnvironmentServer: toLower(TenantSqlServer.outputs.sqlServerName)
    functionAppName: environmentFunctionAppName
    webServerFarmName: environmentWebServerFarmName
    sqlServerName: toLower(EnvironmentManagementSqlServer.outputs.sqlServerName)
  }
}

module EnvironmentFunctiond 'Function_App.bicep' = {
  name: 'Resource-Function-App-Deployment'
  params: {
    location: location
    storageAccountName: CoreServices.outputs.storageAccountName
    appInsightsName: CoreServices.outputs.appInsightsName
    functionDotNetRuntime: 'dotnet'
    functionAppName: resourceManagementFunctionAppName
    webServerFarmName: resourceWebServerFarmName
    sqlServerName: toLower(EnvironmentManagementSqlServer.outputs.sqlServerName)
    tenantEnvironmentServer: toLower(TenantSqlServer.outputs.sqlServerName)
  }
}
