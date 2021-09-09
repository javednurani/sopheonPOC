
param location string = resourceGroup().location

@description('Name of the Azure Function App')
param functionAppName string = '^EnvironmentsFunctionAppName^'

param serverFarmId string = '/subscriptions/1c4bef1d-8a40-4a6d-96d6-764bb466ac46/resourceGroups/Stratus-Dev/providers/Microsoft.Web/serverfarms/ASP-StratusDev-a1f1'

resource EnvironmentsFunctionApp 'Microsoft.Web/sites@2021-01-15' = {
  location: location
  name: functionAppName
  kind: 'functionapp'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: '${functionAppName}.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${functionAppName}.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverFarmId
    reserved: false
    isXenon: false
    hyperV: false
    siteConfig: {
      numberOfWorkers: 1
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 1
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    keyVaultReferenceIdentity: 'SystemAssigned'
    httpsOnly: false
    redundancyMode: 'None'
    storageAccountRequired: false
  }
}

resource BasicPublishingCredsPoliciesFtp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-01-15' = {
  name: '${functionAppName}/ftp'
  properties: {
    allow: true
  }
}

resource BasicPublishingCredsPoliciesScm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-01-15' = {
  name: '${functionAppName}/scm'
  properties: {
    allow: true
  }
}

resource FunctionAppConfig 'Microsoft.Web/sites/config@2021-01-15' = {
  name: '${functionAppName}/web'
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
    ]
    netFrameworkVersion: 'v5.0'
    phpVersion: '5.6'
    requestTracingEnabled: false
    remoteDebuggingEnabled: false
    remoteDebuggingVersion: 'VS2019'
    httpLoggingEnabled: false
    acrUserManageIdentityCreds: false
    logsDirectorySizeLimit: 35
    detailedErrorLoggingEnabled: false
    publishingUsername: '$stratus-dev'
    azureStorageAccounts: {}
    scmType: 'None'
    use32BitWorkerProcess: true
    webSocketsEnabled: false
    alwaysOn: false
    managedPipelineMode: 'Integrated'
    virtualApplications: [
      {
        virtualPath: '/'
        physicalPath: 'site\\wwwroot'
        preloadEnabled: false
      }
    ]
    loadBalancing: 'LeastRequests'
    experiments:{
      rampUpRules: []
    }
    autoHealEnabled: false
    vnetRouteAllEnabled: false
    vnetPrivatePortsCount: 0
    localMySqlEnabled: false
    managedServiceIdentity: 6590
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
    functionAppScaleLimit: 200
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 1
  }
}

resource FunctionAppHostNameBindings 'Microsoft.Web/sites/hostNameBindings@2021-01-15' = {
  name: '${functionAppName}/${functionAppName}.azurewebsites.net'
  properties: {
    siteName: functionAppName
    hostNameType: 'Verified'
  }
}
