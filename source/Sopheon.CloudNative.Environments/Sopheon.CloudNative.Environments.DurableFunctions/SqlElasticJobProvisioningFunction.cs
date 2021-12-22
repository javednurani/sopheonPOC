using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sopheon.CloudNative.Environments.DurableFunctions.Configuration;

namespace Sopheon.CloudNative.Environments.DurableFunctions
{
   public class SqlElasticJobProvisioningFunction
   {
      private readonly ProvisioningState[] _activeProvisioningStates = new ProvisioningState[]
      {
            ProvisioningState.Accepted,
            ProvisioningState.Running
      };

      private readonly List<string> InstancePrefixes = new List<string>() { nameof(RunElasticJobs) };
      private const string SqlEnvironmentUri = ".database.windows.net";

      private readonly EnvironmentContext _dbContext;
      private readonly Lazy<IAzure> _azureApi;
      private readonly IConfiguration _configuration;

      public SqlElasticJobProvisioningFunction(EnvironmentContext dbContext,
          Lazy<IAzure> azureApi,
          IConfiguration configuration)
      {
         _dbContext = dbContext;
         _azureApi = azureApi;
         _configuration = configuration;
      }

      [FunctionName(nameof(RunElasticJobsTask))]
      public async Task<List<string>> RunElasticJobsTask([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
      {
         // Setup variables...
         var outputs = new List<string>();
         string targetResourceGroupName = _configuration["AzResourceGroupName"];
         string startTime = DateTimeOffset.UtcNow.ToString("G");

         //STEP 1: Collect SQL Database resources...
         List<Domain.Models.Resource> currentSqlServerCount = 
            await context.CallActivityAsync<List<Domain.Models.Resource>>(nameof(ElasticJobsTask_GetServerResources), null);
         List<Task<List<string>>> tasks = new List<Task<List<string>>>();

         log.LogInformation($"Creating Elastic Job with start time: '{startTime}'");
         foreach(Domain.Models.Resource resource in currentSqlServerCount)
         {
            string targetedSqlServerName = resource.Uri.Replace(SqlEnvironmentUri, string.Empty);
            ElasticJobConfiguration config = new ElasticJobConfiguration()
            {
               TargetedSqlServerName = targetedSqlServerName,
               ScheduledStartTime = startTime,
               ElasticJobAgentServerName = _configuration["ElasticJobAgentServerName"],
               ResourceGroupName = targetResourceGroupName,
               ElasticJobAgentName = _configuration["ElasticJobAgentName"]
            };

            string deploymentName = await context.CallActivityAsync<string>(nameof(ElasticJobsTask_DeployElasticJob), config);

            tasks.Add(context.CallSubOrchestratorAsync<List<string>>(nameof(MonitorElasticJobsDeployment), deploymentName));
         }        

         List<string>[] deployedDatabases = await Task.WhenAll(tasks);

         if (deployedDatabases.Any())
         {
            await context.CallActivityAsync(nameof(ElasticJobAgentTask_ReportResultsOfDeployment), deployedDatabases);
         }

         return outputs;
      }

      [FunctionName(nameof(ElasticJobsTask_GetServerResources))]
      public async Task<List<Domain.Models.Resource>> ElasticJobsTask_GetServerResources([ActivityTrigger] IDurableActivityContext context, ILogger log)
      {
         List<Domain.Models.Resource> serverResources = await _dbContext.Resources
            .Where(resource => resource.DomainResourceTypeId == (int)Domain.Enums.ResourceTypes.TenantAzureSqlServer)
            .AsNoTracking().ToListAsync();             

         return serverResources;
      }

      [FunctionName(nameof(ElasticJobsTask_DeployElasticJob))]
      public async Task<string> ElasticJobsTask_DeployElasticJob([ActivityTrigger] IDurableActivityContext context, ILogger log,
          [Blob("armtemplates/ElasticJobAgent/ElasticJobAgent_EFMigration.json", Connection = "AzureWebJobsStorage")] string jsonTemplateData,
          [Blob("efmigrations/AppMigrations/products_migration.sql", Connection = "AzureWebJobsStorage")] string sqlCommandText)
      {

         ElasticJobConfiguration config = context.GetInput<ElasticJobConfiguration>();
         string adminLoginEnigma = _configuration["SqlServerAdminEnigma"]; // Pull admin enigma from app config (user secrets or key vault)

         jsonTemplateData = jsonTemplateData
            .Replace("^ElasticJobAgentServerName^", config.ElasticJobAgentServerName)
            .Replace("^ElasticJobAgentName^", config.ElasticJobAgentName)
            .Replace("^TargetSqlServerName^", config.TargetedSqlServerName)
            .Replace("^ScheduledStartTime^", config.ScheduledStartTime)
            .Replace("^SqlCommandText^", sqlCommandText)
            .Replace("^JobUserEnigma^", adminLoginEnigma)
            .Replace("^MasterUserEnigma^", adminLoginEnigma)            ;

         string deploymentName = $"{nameof(SqlElasticJobProvisioningFunction)}_Deployment_{DateTime.UtcNow:yyyyMMddTHHmmss}";
         log.LogInformation($"Creating new deployment: {deploymentName}");

         IDeployment deployment = await _azureApi.Value.Deployments
            .Define(deploymentName)
            .WithExistingResourceGroup(config.ResourceGroupName)
            .WithTemplate(jsonTemplateData)
            .WithParameters("{ }")
            .WithMode(DeploymentMode.Incremental)
            .BeginCreateAsync();

         return deploymentName;
      }

      [FunctionName(nameof(MonitorElasticJobsDeployment))]
      public async Task<List<string>> MonitorElasticJobsDeployment([OrchestrationTrigger] IDurableOrchestrationContext monitorContext, ILogger log)
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
         //IDeployment result = await _azureApi.Value.Deployments.GetByName(deploymentName).RefreshAsync();

         //if (result.ProvisioningState != ProvisioningState.Succeeded)
         //{
         //   return (result.ProvisioningState.Value, new List<string>());
         //}

         //var succesfulSqlDeployments = result.DeploymentOperations.List().ToList();
         //succesfulSqlDeployments = succesfulSqlDeployments
         //    .Where(operation => ("Microsoft.Sql/servers/databases".Equals(operation.TargetResource?.ResourceType, StringComparison.OrdinalIgnoreCase)) &&
         //        ProvisioningState.Succeeded.Value.Equals(operation.ProvisioningState, StringComparison.OrdinalIgnoreCase))
         //    .ToList();


         //List<string> partialConnectionStrings = succesfulSqlDeployments
         //    .Select(operation => {
         //       var list = operation.TargetResource.ResourceName.Split('/');
         //       return $"Server=tcp:{list[0].ToLower()}.database.windows.net,1433;Database={list[1].ToLower()};Encrypt=true;Connection Timeout=30;";
         //    })
         //    .ToList();

         return ("Success", new List<string>());
      }

      [FunctionName(nameof(ElasticJobAgentTask_ReportResultsOfDeployment))]
      public async Task ElasticJobAgentTask_ReportResultsOfDeployment([ActivityTrigger] List<string>[] databaseDeployments, ILogger log)
      {
         
      }

      #region Entry Functions
      [FunctionName("ElasticJobAgentTask_Http_RunElasticJobs")]
      public async Task<HttpResponseMessage> RunElasticJobs(
          //[HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "orchestrators/{functionName}/{instanceIdPrefix}")] HttpRequestMessage req,
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
          [DurableClient] IDurableOrchestrationClient starter,
          ILogger log)
      {
         string functionName = nameof(RunElasticJobsTask);
         bool existingInstancesQueryResult = await IsOrchestrationRunning(starter, log);

         if (!existingInstancesQueryResult)
         {
            // An instance with the specified ID prefix doesn't exist or an existing one stopped running, create one.
            var instanceId = await starter.StartNewAsync<string>(functionName, $"{functionName}_{Guid.NewGuid()}", null);
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
      #endregion

      #region Local Methods
      private async Task<bool> IsOrchestrationRunning(IDurableOrchestrationClient starter, ILogger log)
      {
         foreach (string instancePrefix in InstancePrefixes)
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