@description('The name of the Storage Account')
param storageAccountName string = '^StorageAccountName^'

@description('the name of the App Service')
param webAppName string = '^AppServiceName^'

@description('the name of the App Insights for SPA')
param appInsightsName string = '^AppInsightsName^'

@description('The SKU of App Service Plan.')
param sku string = 'S1'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

param env string = '^Environment^'

var appServicePlanPortalName_var = '${webAppName}-AppServicePlan'

resource AppService_PlanPortal 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanPortalName_var
  location: location
  sku: {
    name: sku
  }
  kind: 'windows'
  properties: {
    reserved: true
  }
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: appInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
resource storageAccount 'Microsoft.Storage/storageAccounts@2021-04-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
}

resource EnvironmentFunctionApp_Storage_BlobService 'Microsoft.Storage/storageAccounts/blobServices@2021-04-01' = {
  name: '${storageAccount.name}/default'
  properties: {
    deleteRetentionPolicy: {
      enabled: false
    }
  }
}

resource StaticWebpage_Storage_BlobService_AppLogsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${EnvironmentFunctionApp_Storage_BlobService.name}/applogs'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [
    storageAccount
  ]
}

resource StaticWebpage_Storage_BlobService_ServerLogsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${EnvironmentFunctionApp_Storage_BlobService.name}/serverlogs'
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
  dependsOn: [
    storageAccount
  ]
}

resource ProductManagementWebApp 'Microsoft.Web/sites@2021-02-01' = {
  name: webAppName
  location: 'West US'
  kind: 'app'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${webAppName}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${webAppName}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: AppService_PlanPortal.id
    reserved: false
    isXenon: false
    hyperV: false
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: true
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 1
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: '${appInsights.properties.InstrumentationKey}'
        }
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'development'
        }
      ]
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: true
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_StratusProductManagement_Dev_name_web 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: ProductManagementWebApp
  name: 'web'
  properties: {
    numberOfWorkers: 1
    defaultDocuments: [
      'Default.htm'
      'Default.html'
      'Default.asp'
      'index.htm'
      'index.html'
      'iisstart.htm'
      'default.aspx'
      'index.php'
      'hostingstart.html'
    ]
    netFrameworkVersion: 'v5.0'
    requestTracingEnabled: true
    requestTracingExpirationTime: '12/31/9999 11:59:00 PM'
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: true
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: true
    publishingUsername: '$StratusProductManagement-${env}'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: true
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: true
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments: {
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    localMySqlEnabled: false
    ipSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictions: [
      {
        ipAddress: 'Any'
        action: 'Allow'
        priority: 1
        name: 'Allow all'
        description: 'Allow all access'
      }
    ]
    scmIpSecurityRestrictionsUseMain: false
    http20Enabled: false
    minTlsVersion: '1.2'
    scmMinTlsVersion: '1.0'
    ftpsState: 'AllAllowed'
    preWarmedInstanceCount: 0
    functionAppScaleLimit: 0
    healthCheckPath: '/health'
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 1
    azureStorageAccounts: {}
  }
}

resource sites_StratusProductManagement_Dev_name_sites_StratusProductManagement_Dev_name_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2021-02-01' = {
  parent: ProductManagementWebApp
  name: '${ProductManagementWebApp.name}.azurewebsites.net'
  properties: {
    siteName: 'StratusProductManagement-${env}' //TODO: Configure for TokenReplace
    hostNameType: 'Verified'
  }
}
