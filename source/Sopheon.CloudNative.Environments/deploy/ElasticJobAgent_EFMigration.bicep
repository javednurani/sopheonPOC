param credentialsJobUserPassword string = '^JobUserEnigma^'
param credentialsMasterUserPassword string = '^MasterUserEnigma^'

param elasticJobAgentServerName string = '^ElasticJobAgentServerName^'
param elasticJobAgentName string = '^ElasticJobAgentName^'
param targetSqlServerName string = '^TargetSqlServerName^'
param scheduledStartTime string = '^ScheduledStartTime^'
param elasticJobStepCommandText string = '^SqlCommandText^'
@description('No need to set, this is setup to use utcNow() when generated on deploy')
param dateTime string = uniqueString(utcNow())

resource elasticJobServer 'Microsoft.Sql/servers@2021-05-01-preview' = {
  name: elasticJobAgentServerName
  location: resourceGroup().location
}

resource elasticjobDatabase 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  name: '${elasticJobServer.name}/JobDatabase'
  location: resourceGroup().location
}

resource elasticJobAgent 'Microsoft.Sql/servers/jobAgents@2021-02-01-preview' = {
  name: '${elasticJobServer.name}/${elasticJobAgentName}'
  location: 'westus'
  sku: {
    name: 'Agent'
    capacity: 100
  }
  properties: {
    databaseId: elasticjobDatabase.id
  }
}

resource elasticJobAgentCredentialJobUser 'Microsoft.Sql/servers/jobAgents/credentials@2021-02-01-preview' existing = {
  parent: elasticJobAgent
  name: 'jobuser'
}

resource elasticJobAgentCredentialMasterUser 'Microsoft.Sql/servers/jobAgents/credentials@2021-02-01-preview' existing = {
  parent: elasticJobAgent
  name: 'masteruser'
}

resource elasticJobAgentTargetGroupServerGroup 'Microsoft.Sql/servers/jobAgents/targetGroups@2021-02-01-preview' = {
  parent: elasticJobAgent
  name: 'ServerGroup'
  properties: {
    members: [
      {
        membershipType: 'Include'
        type: 'SqlServer'
        serverName: targetSqlServerName
        refreshCredential: elasticJobAgentCredentialMasterUser.id
      }
    ]
  }
}

resource elasticJobAgentExampleJobForBuild 'Microsoft.Sql/servers/jobAgents/jobs@2021-02-01-preview' = {
  parent: elasticJobAgent
  name: uniqueString(elasticJobAgent.name, elasticJobAgentServerName, dateTime)
  properties: {
    schedule: {
      startTime: scheduledStartTime
      endTime: '12/31/9999 11:59:59 AM'
      type: 'Once'
      enabled: true
    }
  }
}

resource elasticJobAgentExampleJobForBuildStep1 'Microsoft.Sql/servers/jobAgents/jobs/steps@2021-02-01-preview' = {
  parent: elasticJobAgentExampleJobForBuild
  name: 'step1'
  properties: {
    stepId: 1
    targetGroup: elasticJobAgentTargetGroupServerGroup.id
    credential: elasticJobAgentCredentialJobUser.id
    action: {
      type: 'TSql'
      source: 'Inline'
      value: elasticJobStepCommandText
    }
    executionOptions: {
      timeoutSeconds: 3600
      retryAttempts: 3
      initialRetryIntervalSeconds: 1
      maximumRetryIntervalSeconds: 120
      retryIntervalBackoffMultiplier: 2
    }
  }
  dependsOn: [
    elasticJobAgent
  ]
}
