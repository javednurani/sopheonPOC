@description('The name of the Storage Account')
param storageAccountName string = '^StorageAccountName^'

@description('the name of the App Service')
param webAppName string = '^AppServiceName^'

@description('the name of the App Insights for SPA')
param appInsightsName string = '^AppInsightsName^'

@description('The SKU CODE of App Service Plan.')
param skuCode string = 'S1'

@description('The SKU name of the App Service Plan.')
param sku string = 'Standard'

@description('The location of where to deploy the resource')
param location string = resourceGroup().location

param env string = '^Environment^'

var appServicePlanPortalName_var = '${webAppName}-appserviceplan'

resource AppService_PlanPortal 'Microsoft.Web/serverfarms@2020-06-01' = {
  name: appServicePlanPortalName_var
  location: location
  sku: {
    tier: sku
    name: skuCode
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
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
    supportsHttpsTrafficOnly: true
    encryption: {
      keySource: 'Microsoft.Storage'
      services: {
        blob: {
          keyType: 'Account'
          enabled: true
        }
        file: {
          keyType: 'Account'
          enabled: true
        }
      }
    }
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
  dependsOn: [
    storageAccount
  ]
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
}

resource StaticWebpage_Storage_BlobService_ServerLogsContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-04-01' = {
  name: '${EnvironmentFunctionApp_Storage_BlobService.name}/serverlogs'
  dependsOn: [
    storageAccount
  ]
  properties: {
    defaultEncryptionScope: '$account-encryption-key'
    denyEncryptionScopeOverride: false
    publicAccess: 'None'
  }
}

resource ProductManagementWebApp 'Microsoft.Web/sites@2021-02-01' = {
  name: webAppName
  location: 'West US'
  kind: 'app'
  properties: {
    enabled: true    
    serverFarmId: AppService_PlanPortal.id
    reserved: false
    isXenon: false
    hyperV: false
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
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: true
      http20Enabled: false
      functionAppScaleLimit: 0
      minimumElasticInstanceCount: 1
      netFrameworkVersion: 'v6.0'      
      appSettings: [
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: appInsights.properties.InstrumentationKey
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ApplicationInsightsAgent_EXTENSION_VERSION'
          value: '~2'
        }
        {
          name: 'XDT_MicrosoftApplicationInsights_Mode'
          value: 'default'
        }
        {
          name: 'ServiceUrls:GetEnvironmentResourceBindingUri'
          value: 'http://${toLower(resourceGroup().name)}.azurewebsites.net/GetEnvironmentResourceBindingUri'
        }
        {
          name: 'WEBSITE_HTTPLOGGING_RETENTION_DAYS'
          value: '90'
        }
        {
          name: 'WEBSITE_HEALTHCHECK_MAXPINGFAILURES'
          value: '10'
        }
        // {
        //   name: 'ASPNETCORE_ENVIRONMENT'
        //   value: 'development'
        // }
      ]
    }
  }
}

resource sites_StratusProductManagement_Dev_name_web 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: ProductManagementWebApp
  name: 'web'
  properties: {
    numberOfWorkers: 1    
    netFrameworkVersion: 'v6.0'
    requestTracingEnabled: true
    requestTracingExpirationTime: '12/31/9999 11:59:00 PM'
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: false
    acrUseManagedIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$StratusProductManagement-${env}'
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: true
    managedPipelineMode: 'Integrated'
    loadBalancing: 'LeastRequests'
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    localMySqlEnabled: false
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
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: true
      }
    ]    
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
    experiments: {
      rampUpRules: []
    }
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
