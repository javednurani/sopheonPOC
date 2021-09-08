@description('The name of the Environment Function App')
param environmentFunctionApp_Name string = '^EnvironmentFunctionAppName^'

@description('The name of the SQL Server')
param sqlServer_name string = '^SQLServerName^'

@description('Sql Server Password')
@secure()
param sqlServer_Enigma string = ''

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

// SQL Server module
module SqlServer 'SQLServer_Database_Template.bicep' = {
  name: sqlServer_name
  params: {
    location: location
    administratorLoginEngima: sqlServer_Enigma
  }
}

module EnvironmentFunction 'Environments_Function_App.bicep' = {
  name: environmentFunctionApp_Name
  params: {
    location: location
  }
}

