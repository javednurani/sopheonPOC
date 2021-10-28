param sites_StratusProductManagement_Dev_name string = 'StratusProductManagement-Dev'
param serverfarms_StratusProductManagement_AppPlan_Dev_externalid string = '/subscriptions/1c4bef1d-8a40-4a6d-96d6-764bb466ac46/resourceGroups/Accolade-Next-Gen/providers/Microsoft.Web/serverfarms/StratusProductManagement-AppPlan-Dev'

resource sites_StratusProductManagement_Dev_name_resource 'Microsoft.Web/sites@2021-02-01' = {
  name: sites_StratusProductManagement_Dev_name
  location: 'West US'
  tags: {
    'Environment Type': 'Research'
    Owner: 'CloudTeam-1'
    'Review Date': '06-30-21'
  }
  kind: 'app'
  properties: {
    enabled: true
    hostNameSslStates: [
      {
        name: 'stratusproductmanagement-dev.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: 'stratusproductmanagement-dev.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Repository'
      }
    ]
    serverFarmId: serverfarms_StratusProductManagement_AppPlan_Dev_externalid
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
      appSettings: {
        
      }
    }
    scmSiteAlsoStopped: false
    clientAffinityEnabled: true
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: 'F50C4A4633DCAF18B0C09B554884823C47C074E58674E017191D38260A145C6A'
    containerSize: 0
    dailyMemoryTimeQuota: 0
    httpsOnly: false
    redundancyMode: 'None'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

resource sites_StratusProductManagement_Dev_name_ftp 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_StratusProductManagement_Dev_name_resource
  name: 'ftp'
  properties: {
    allow: true
  }
}

resource sites_StratusProductManagement_Dev_name_scm 'Microsoft.Web/sites/basicPublishingCredentialsPolicies@2021-02-01' = {
  parent: sites_StratusProductManagement_Dev_name_resource
  name: 'scm'
  properties: {
    allow: true
  }
}

resource sites_StratusProductManagement_Dev_name_web 'Microsoft.Web/sites/config@2021-02-01' = {
  parent: sites_StratusProductManagement_Dev_name_resource
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
    publishingUsername: '$StratusProductManagement-Dev'
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
    healthCheckPath: '/swagger/ui'
    functionsRuntimeScaleMonitoringEnabled: false
    minimumElasticInstanceCount: 1
    azureStorageAccounts: {}
  }
}

resource sites_StratusProductManagement_Dev_name_sites_StratusProductManagement_Dev_name_azurewebsites_net 'Microsoft.Web/sites/hostNameBindings@2021-02-01' = {
  parent: sites_StratusProductManagement_Dev_name_resource
  name: '${sites_StratusProductManagement_Dev_name}.azurewebsites.net'
  properties: {
    siteName: 'StratusProductManagement-Dev' //TODO: Configure for TokenReplace
    hostNameType: 'Verified'
  }
}

resource sites_StratusProductManagement_Dev_name_Microsoft_AspNetCore_AzureAppServices_SiteExtension 'Microsoft.Web/sites/siteextensions@2021-02-01' = {
  parent: sites_StratusProductManagement_Dev_name_resource
  name: 'Microsoft.AspNetCore.AzureAppServices.SiteExtension'
}
