namespace Sopheon.CloudNative.Environments.DurableFunctions
{
   public class SqlDatabaseProvisioningFunction
   {
      private readonly ProvisioningState[] _activeProvisioningStates = new ProvisioningState[]
      {
            ProvisioningState.Accepted,
            ProvisioningState.Running
      };

      private readonly List<string> InstancePrefixes = new List<string>() { nameof(RunSingle), nameof(RunTimer) };

      private readonly EnvironmentContext _dbContext;
      private readonly Lazy<IAzure> _azureApi;
      private readonly IConfiguration _configuration;

      public SqlDatabaseProvisioningFunction(EnvironmentContext dbContext,
          Lazy<IAzure> azureApi,
          IConfiguration configuration)
      {
         _dbContext = dbContext;
         _azureApi = azureApi;
         _configuration = configuration;
      }

      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding))]
      public async Task<List<string>> MaintainSqlDatabasePoolForOnboarding([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
      {
         var outputs = new List<string>();

         int currentUnallocatedSqlDatabaseCount = await context.CallActivityAsync<int>(nameof(MaintainSqlDatabasePoolForOnboarding_GetUnallocatedDatabases), null);
         string targetResourceGroupName = _configuration["TargetResourceGroupName"];
         int minimumBufferCapacity = _configuration.GetValue<int>("MinimumBufferCapacity");

         if (currentUnallocatedSqlDatabaseCount < minimumBufferCapacity)
         {
            log.LogInformation($"Unalloacted Database count was lower than 50. Actual count:{currentUnallocatedSqlDatabaseCount}");

            string deploymentName = await context.CallActivityAsync<string>(nameof(MaintainSqlDatabasePoolForOnboarding_DeployElasticPoolAndDatabases), targetResourceGroupName);

            List<Domain.Models.Resource> deployedResources = await context.CallSubOrchestratorAsync<List<Domain.Models.Resource>>(nameof(MonitorDeployment), deploymentName);

            if (deployedResources.Any(res => res.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.AzureSqlDb))
            {
               await context.CallActivityAsync(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases),
                  deployedResources.Where(res => res.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.AzureSqlDb));
            }

            if (deployedResources.Any(res => res.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.TenantAzureSqlServer))
            {
               await context.CallActivityAsync(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedSqlServers),
                  deployedResources.Where(res => res.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.TenantAzureSqlServer));
            }

            return outputs;
         }

         log.LogInformation($"Unalloacted Database count was higher than 50. Actual count:{currentUnallocatedSqlDatabaseCount}");

         return outputs;
      }

      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding_GetUnallocatedDatabases))]
      public async Task<int> MaintainSqlDatabasePoolForOnboarding_GetUnallocatedDatabases([ActivityTrigger] string name, ILogger log)
      {
         int unallocatedDatabaseCount = await _dbContext.Resources
             .Where(resource => resource.DomainResourceTypeId == 1 &&
                 !_dbContext.DedicatedEnvironmentResources.Any(dedicatedResource => dedicatedResource.ResourceId == resource.Id))
             .CountAsync();

         return unallocatedDatabaseCount;
      }

      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding_DeployElasticPoolAndDatabases))]
      public async Task<string> MaintainSqlDatabasePoolForOnboarding_DeployElasticPoolAndDatabases([ActivityTrigger] IDurableActivityContext context,
          [Blob("armtemplates/ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json", Connection = "AzureWebJobsStorage")] string jsonTemplateData, ILogger log)
      {

         string resourceGroupName = context.GetInput<string>();
         string sqlServerName = _configuration["AzSqlServerName"];
         string adminLoginEnigma = _configuration["SqlServerAdminEnigma"]; // Pull admin enigma from app config (user secrets or key vault)

         jsonTemplateData = jsonTemplateData
            .Replace("^SqlServerName^", sqlServerName)
            .Replace("^SqlAdminEngima^", adminLoginEnigma);

         string deploymentName = $"{nameof(SqlDatabaseProvisioningFunction)}_Deployment_{DateTime.UtcNow:yyyyMMddTHHmmss}";
         log.LogInformation($"Creating new deployment: {deploymentName}");

         IDeployment deployment = await _azureApi.Value.Deployments
            .Define(deploymentName)
            .WithExistingResourceGroup(resourceGroupName)
            .WithTemplate(jsonTemplateData)
            .WithParameters("{ }")
            .WithMode(DeploymentMode.Incremental)
            .BeginCreateAsync();

         return deploymentName;
      }

      /// <summary>
      /// Monitors the active deployment of the buffer databases
      /// </summary>
      /// <param name="monitorContext"></param>
      /// <param name="log"></param>
      /// <returns>True if the deployment was successful</returns>
      [FunctionName(nameof(MonitorDeployment))]
      public async Task<List<Domain.Models.Resource>> MonitorDeployment([OrchestrationTrigger] IDurableOrchestrationContext monitorContext, ILogger log)
      {
         int pollingDelaySeconds = 10;
         log = monitorContext.CreateReplaySafeLogger(log);
         List<Domain.Models.Resource> resources = new List<Domain.Models.Resource>();

         string deploymentName = monitorContext.GetInput<string>();

         DateTime endTime = monitorContext.CurrentUtcDateTime.AddHours(1); // Deployment Monitor Timeout

         while (monitorContext.CurrentUtcDateTime < endTime)
         {
            log.LogInformation($"Polling status of deployment {deploymentName}");

            (string databaseState, List<Domain.Models.Resource> databaseOps) = await monitorContext.CallActivityAsync<(string, List<Domain.Models.Resource>)>(nameof(GetDeploymentResultsForSQLDatabases), deploymentName);
            (string serverState, List<Domain.Models.Resource> serverOps) = await monitorContext.CallActivityAsync<(string, List<Domain.Models.Resource>)>(nameof(GetDeploymentResultsForSQLServers), deploymentName);

            if (!_activeProvisioningStates.Any(activeState => activeState.Value.Equals(databaseState, StringComparison.OrdinalIgnoreCase)) &&
               !_activeProvisioningStates.Any(state => state.Value.Equals(serverState, StringComparison.OrdinalIgnoreCase)))
            {
               resources.AddRange(databaseOps);
               resources.AddRange(serverOps);
               return resources;
            }
            else
            {
               DateTime nextCheckpoint = monitorContext.CurrentUtcDateTime.AddSeconds(pollingDelaySeconds);

               await monitorContext.CreateTimer(nextCheckpoint, CancellationToken.None); // Wait for the next checkpoint
            }
         }

         log.LogInformation($"Monitor expiring.");
         return resources;
      }

      [FunctionName(nameof(GetDeploymentResultsForSQLDatabases))]
      public async Task<(string State, List<Domain.Models.Resource> DeployedDatabases)> GetDeploymentResultsForSQLDatabases([ActivityTrigger] string deploymentName)
      {
         IDeployment result = await _azureApi.Value.Deployments.GetByName(deploymentName).RefreshAsync();

         if (result.ProvisioningState != ProvisioningState.Succeeded)
         {
            return (result.ProvisioningState.Value, new List<Domain.Models.Resource>());
         }

         List<IDeploymentOperation> succesfulSqlDatabaseDeployments = result.DeploymentOperations.List().ToList();
         succesfulSqlDatabaseDeployments = succesfulSqlDatabaseDeployments
             .Where(operation => ("Microsoft.Sql/servers/databases".Equals(operation.TargetResource?.ResourceType, StringComparison.OrdinalIgnoreCase)) &&
                 ProvisioningState.Succeeded.Value.Equals(operation.ProvisioningState, StringComparison.OrdinalIgnoreCase))
             .ToList();

         List<Domain.Models.Resource> partialConnectionStrings = succesfulSqlDatabaseDeployments
             .Select(operation => {
                var list = operation.TargetResource.ResourceName.Split('/');
                return new Domain.Models.Resource
                {
                   DomainResourceTypeId = (int)Domain.Enums.ResourceTypes.AzureSqlDb,
                   Uri = $"Server=tcp:{list[0].ToLower()}.database.windows.net,1433;Database={list[1].ToLower()};Encrypt=true;Connection Timeout=30;"
                };
             })
             .ToList();

         return (result.ProvisioningState.Value, partialConnectionStrings);
      }

      [FunctionName(nameof(GetDeploymentResultsForSQLServers))]
      public async Task<(string State, List<Domain.Models.Resource> DeployedSqlServers)> GetDeploymentResultsForSQLServers([ActivityTrigger] string deploymentName)
      {
         IDeployment result = await _azureApi.Value.Deployments.GetByName(deploymentName).RefreshAsync();

         if (result.ProvisioningState != ProvisioningState.Succeeded)
         {
            return (result.ProvisioningState.Value, new List<Domain.Models.Resource>());
         }

         List<IDeploymentOperation> succesfulSqlServerDeployments = result.DeploymentOperations.List().ToList();
         succesfulSqlServerDeployments = succesfulSqlServerDeployments
             .Where(operation => ("Microsoft.Sql/servers".Equals(operation.TargetResource?.ResourceType, StringComparison.OrdinalIgnoreCase)) &&
                 ProvisioningState.Succeeded.Value.Equals(operation.ProvisioningState, StringComparison.OrdinalIgnoreCase))
             .ToList();

         List<Domain.Models.Resource> sqlServerResources = succesfulSqlServerDeployments
             .Select(operation => {
                var list = operation.TargetResource.ResourceName.Split('/');
                return new Domain.Models.Resource 
                {
                   DomainResourceTypeId = (int)Domain.Enums.ResourceTypes.TenantAzureSqlServer,
                   Uri = $"{list[0].ToLower()}.database.windows.net"
                };
             })
             .ToList();

         return (result.ProvisioningState.Value, sqlServerResources);
      }


      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases))]
      public async Task MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases([ActivityTrigger] List<Domain.Models.Resource> databaseDeployments, ILogger log)
      {
         foreach (Domain.Models.Resource resource in databaseDeployments)
         {
            if (!_dbContext.Resources.Any(r => r.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.AzureSqlDb && resource.Uri == r.Uri))
            {
               _dbContext.Resources.Add(resource);
            }
         }

         await _dbContext.SaveChangesAsync();
      }

      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedSqlServers))]
      public async Task MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedSqlServers([ActivityTrigger] List<Domain.Models.Resource> sqlServerDeployments, ILogger log)
      {
         foreach (Domain.Models.Resource server in sqlServerDeployments)
         {
            if (!_dbContext.Resources.Any(r => r.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.TenantAzureSqlServer && server.Uri == r.Uri))
            {
               _dbContext.Resources.Add(server);
            }
         }

         await _dbContext.SaveChangesAsync();
      }

      #region Entry Functions
      [FunctionName("MaintainSqlDatabasePoolForOnboarding_HttpStartSingle")]
      public async Task<HttpResponseMessage> RunSingle(
          //[HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "orchestrators/{functionName}/{instanceIdPrefix}")] HttpRequestMessage req,
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
          [DurableClient] IDurableOrchestrationClient starter,
          ILogger log)
      {
         string functionName = nameof(MaintainSqlDatabasePoolForOnboarding);
         bool existingInstancesQueryResult = await IsOrchestrationRunning(starter, log);

         if (!existingInstancesQueryResult)
         {
            // An instance with the specified ID prefix doesn't exist or an existing one stopped running, create one.
            var instanceId = await starter.StartNewAsync<string>(functionName, $"{functionName}_{Guid.NewGuid()}");
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
         }
         else
         {            
            // An instance with the specified ID prefix exists or an existing one still running, don't create one.
            //string runningInstances = string.Join(", ", existingInstancesQueryResult.DurableOrchestrationState.Select(dos => $"{dos.InstanceId}"));
            return new HttpResponseMessage(HttpStatusCode.Conflict)
            {
               Content = new StringContent($"An instance is already running. Details: [{/*runningInstances*/string.Empty}]")
            };
         }
      }

      [FunctionName("MaintainSqlDatabasePoolForOnboarding_Timer")]
      public async Task RunTimer(
           [TimerTrigger("%DatabaseBufferTimer%")] TimerInfo myTimer,
           [DurableClient] IDurableOrchestrationClient starter,
           ILogger log)
      {
         string functionName = nameof(MaintainSqlDatabasePoolForOnboarding);

         bool existingInstancesQueryResult = await IsOrchestrationRunning(starter, log);

         if (!existingInstancesQueryResult)
         {
            // An instance with the specified ID prefix doesn't exist or an existing one stopped running, create one.
            var instanceId = await starter.StartNewAsync<string>(functionName, $"{functionName}_{Guid.NewGuid()}");
            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
         }
      }
      #endregion

      #region Local Methods
      private async Task<bool> IsOrchestrationRunning(IDurableOrchestrationClient starter, ILogger log)
      {
         foreach(string instancePrefix in InstancePrefixes)
         {
            OrchestrationStatusQueryResult existingInstancesQueryResult = await starter.ListInstancesAsync(new OrchestrationStatusQueryCondition
            {
               InstanceIdPrefix = instancePrefix,
               RuntimeStatus = new OrchestrationRuntimeStatus[]
               {
                  OrchestrationRuntimeStatus.Pending,
                  OrchestrationRuntimeStatus.Running,
                  OrchestrationRuntimeStatus.ContinuedAsNew
               }
            }, CancellationToken.None);

            if (existingInstancesQueryResult.DurableOrchestrationState.Any())
            {
               string runningInstances = string.Join(", ", existingInstancesQueryResult.DurableOrchestrationState.Select(dos => $"{dos.InstanceId}"));
               log.LogInformation(runningInstances);
               return true;
            }
         }
         return false;
      }
      #endregion
   }
}