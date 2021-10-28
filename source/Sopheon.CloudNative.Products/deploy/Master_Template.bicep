@description('The name of the Environment Function App')
param webApiProductsApp_Name string = '^WebApiProductsAppName^'

param webApiProductsAppStorage_name string = '^WebApiProductsStorageAccountName^'

param appInsightsName string = '^AppInsightsName^'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

param env string = '^Environment^'

module EnvironmentFunction 'WebApi_App_Service.bicep' = {
  name: 'Function-App-Deployment'
  params: {
    location: location
    storageAccountName: webApiProductsAppStorage_name
    env: env
    appInsightsName: appInsightsName
    webAppName: webApiProductsApp_Name
 }
}

