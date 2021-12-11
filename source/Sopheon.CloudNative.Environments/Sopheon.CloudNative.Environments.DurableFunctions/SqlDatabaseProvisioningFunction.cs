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
         string targetResourceGroupName = _configuration["AzResourceGroupName"];
         int minimumBufferCapacity = _configuration.GetValue<int>("DatabaseBufferCapacity");

         if (currentUnallocatedSqlDatabaseCount < minimumBufferCapacity)
         {
            log.LogInformation($"Unalloacted Database count was lower than {minimumBufferCapacity}. Actual count:{currentUnallocatedSqlDatabaseCount}");

            string deploymentName = await context.CallActivityAsync<string>(nameof(MaintainSqlDatabasePoolForOnboarding_DeployElasticPoolAndDatabases), targetResourceGroupName);

            List<string> deployedDatabases = await context.CallSubOrchestratorAsync<List<string>>(nameof(MonitorDeployment), deploymentName);

            if (deployedDatabases.Any())
            {
               await context.CallActivityAsync(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases), deployedDatabases);
            }

            return outputs;
         }

         log.LogInformation($"Unalloacted Database count was higher than {minimumBufferCapacity}. Actual count:{currentUnallocatedSqlDatabaseCount}");

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
         string tenantSqlServerName = _configuration["AzTenantEnvironmentServer"];
         string adminLoginEnigma = _configuration["SqlServerAdminEnigma"]; // Pull admin enigma from app config (user secrets or key vault)
         string envSqlServerName = _configuration["AzSqlServerName"];
         string envSqlDatabaseName = "EnvironmentManagement";

         jsonTemplateData = jsonTemplateData
            .Replace("^SqlServerName^", tenantSqlServerName)
            .Replace("^SqlAdminEngima^", adminLoginEnigma)
            .Replace("^EnvironmentManagementSQLServerName^", envSqlServerName)
            .Replace("^EnvironmentManagementSQLServerDatabaseName^", envSqlDatabaseName);

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
      public async Task<List<string>> MonitorDeployment([OrchestrationTrigger] IDurableOrchestrationContext monitorContext, ILogger log)
      {
         int pollingDelaySeconds = 10;
         log = monitorContext.CreateReplaySafeLogger(log);

         string deploymentName = monitorContext.GetInput<string>();

         DateTime endTime = monitorContext.CurrentUtcDateTime.AddHours(1); // Deployment Monitor Timeout

         while (monitorContext.CurrentUtcDateTime < endTime)
         {
            log.LogInformation($"Polling status of deployment {deploymentName}");

            (string state, List<string> operations) = await monitorContext.CallActivityAsync<(string, List<string>)>(nameof(GetDeploymentResults), deploymentName);

            if (!_activeProvisioningStates.Any(activeState => activeState.Value.Equals(state, StringComparison.OrdinalIgnoreCase)))
            {
               return operations;
            }
            else
            {
               DateTime nextCheckpoint = monitorContext.CurrentUtcDateTime.AddSeconds(pollingDelaySeconds);

               await monitorContext.CreateTimer(nextCheckpoint, CancellationToken.None); // Wait for the next checkpoint
            }
         }

         log.LogInformation($"Monitor expiring.");
         return new List<string>();
      }

      [FunctionName(nameof(GetDeploymentResults))]
      public async Task<(string State, List<string> DeployedDatabases)> GetDeploymentResults([ActivityTrigger] string deploymentName)
      {
         IDeployment result = await _azureApi.Value.Deployments.GetByName(deploymentName).RefreshAsync();

         if (result.ProvisioningState != ProvisioningState.Succeeded)
         {
            return (result.ProvisioningState.Value, new List<string>());
         }

         var succesfulSqlDeployments = result.DeploymentOperations.List().ToList();
         succesfulSqlDeployments = succesfulSqlDeployments
             .Where(operation => ("Microsoft.Sql/servers/databases".Equals(operation.TargetResource?.ResourceType, StringComparison.OrdinalIgnoreCase)) &&
                 ProvisioningState.Succeeded.Value.Equals(operation.ProvisioningState, StringComparison.OrdinalIgnoreCase))
             .ToList();


         List<string> partialConnectionStrings = succesfulSqlDeployments             
             .Select(operation => {
                var list = operation.TargetResource.ResourceName.Split('/');
                return $"Server=tcp:{list[0].ToLower()}.database.windows.net,1433;Database={list[1].ToLower()};Encrypt=true;Connection Timeout=30;";
                })
             .ToList();

         return (result.ProvisioningState.Value, partialConnectionStrings);
      }

      [FunctionName(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases))]
      public async Task MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases([ActivityTrigger] List<string> databaseDeployments, ILogger log)
      {
         foreach (string partialConnectionString in databaseDeployments)
         {
            if (!_dbContext.Resources.Any(r => r.DomainResourceTypeId == 1 && partialConnectionString == r.Uri))
            {
               _dbContext.Resources.Add(new Domain.Models.Resource()
               {
                  DomainResourceTypeId = 1, // TODO:Enum
                  Uri = partialConnectionString
               });
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