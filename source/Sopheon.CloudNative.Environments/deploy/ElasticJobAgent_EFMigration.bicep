param credentialsJobUserPassword string = '^JobUserEnigma^'
param credentialsMasterUserPassword string = '^MasterUserEnigma^'

param elasticJobAgentServerName string = '^ElasticJobAgentServerName^'
param elasticJobAgentName string = '^ElasticJobAgentName^'
param targetSqlServerName string = '^TargetSqlServerName^'
param scheduledStartTime string = '^ScheduledStartTime^'
param elasticJobStepCommandText string = '^SqlCommandText^'

resource elasticjobDatabase 'Microsoft.Sql/servers/databases@2021-05-01-preview' = {
  name: '${elasticJobAgentServerName}/JobDatabase}'
  location: resourceGroup().location
}

resource elasticJobAgent 'Microsoft.Sql/servers/jobAgents@2021-02-01-preview' = {
  name: '${elasticJobAgentServerName}/${elasticJobAgentName}'
  location: 'westus'
  sku: {
    name: 'Agent'
    capacity: 100
  }
  properties: {
    databaseId: elasticjobDatabase.id
  }
}

resource elasticJobAgentCredentialJobUser 'Microsoft.Sql/servers/jobAgents/credentials@2021-02-01-preview' = {
  parent: elasticJobAgent
  name: 'jobuser'
  properties: {
    username: 'jobuser'
    password: credentialsJobUserPassword
  }
}

resource elasticJobAgentCredentialMasterUser 'Microsoft.Sql/servers/jobAgents/credentials@2021-02-01-preview' = {
  parent: elasticJobAgent
  name: 'masteruser'
  properties: {
    username: 'masteruser'
    password: credentialsMasterUserPassword
  }
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
  name: uniqueString(elasticJobAgent.name, elasticJobAgentServerName)
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
