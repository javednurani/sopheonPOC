@description('The name of the Environment Function App')
param webApiProductsAppName string = '^WebApiProductsAppName^'

param webApiProductsAppStorageName string = '^WebApiProductsStorageAccountName^'

param appInsightsName string = '^AppInsightsName^'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

param env string = '^Environment^'

module EnvironmentFunction 'WebApi_App_Service.bicep' = {
  name: 'Function-App-Deployment'
  params: {
    location: location
    storageAccountName: webApiProductsAppStorageName
    env: env
    appInsightsName: appInsightsName
    webAppName: webApiProductsAppName
 }
}

