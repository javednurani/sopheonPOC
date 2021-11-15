using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sopheon.CloudNative.Environments.Data;

namespace Sopheon.CloudNative.Environments.DurableFunctions
{
    public class SqlDatabaseProvisioningFunction
    {
        private readonly ProvisioningState[] _activeProvisioningStates = new ProvisioningState[]
        {
            ProvisioningState.Accepted,
            ProvisioningState.Running
        };

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
        public async Task<List<string>> MaintainSqlDatabasePoolForOnboarding(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            int currentUnallocatedSqlDatabaseCount = await context.CallActivityAsync<int>(nameof(MaintainSqlDatabasePoolForOnboarding_GetUnallocatedDatabases), null);
            string targetResourceGroupName = _configuration["TargetResourceGroupName"]; // "clcbuffertest";
            int minimumBufferCapacity = _configuration.GetValue<int>("MinimumBufferCapacity"); // 50; // TODO: Parameter

            if (currentUnallocatedSqlDatabaseCount < minimumBufferCapacity)
            {
                string deploymentName = await context.CallActivityAsync<string>(nameof(MaintainSqlDatabasePoolForOnboarding_DeployElasticPoolAndDatabases), targetResourceGroupName);

                List<string> deployedDatabases = await context.CallSubOrchestratorAsync<List<string>> (nameof(MonitorDeployment), deploymentName);

                if(deployedDatabases.Any())
                {
                    await context.CallActivityAsync(nameof(MaintainSqlDatabasePoolForOnboarding_RegisterNewlyCreatedDatabases), deployedDatabases);
                }
            }

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
            [Blob("armtemplates/ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json", Connection = "ARM_Template_BlobStorage_ConnectionString")] string jsonTemplateData,
            ILogger log)
        {
            string resourceGroupName = context.GetInput<string>();
            string sqlServerName = Environment.GetEnvironmentVariable("AzSqlServerName");
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
        /// 
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
                .Select(operation => String.Format("Server={0};Database={1};", operation.TargetResource.ResourceName.Split("/")))
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

        [FunctionName("MaintainSqlDatabasePoolForOnboarding_HttpStartSingle")]
        public async Task<HttpResponseMessage> RunSingle(
            //[HttpTrigger(AuthorizationLevel.Function, methods: "post", Route = "orchestrators/{functionName}/{instanceIdPrefix}")] HttpRequestMessage req,
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            string functionName = nameof(MaintainSqlDatabasePoolForOnboarding);
            string instanceIdPrefix = functionName;

            // Check if an running instance with the specified ID prefix exists.
            OrchestrationStatusQueryResult existingInstancesQueryResult = await starter.ListInstancesAsync(new OrchestrationStatusQueryCondition
            {
                InstanceIdPrefix = instanceIdPrefix,
                RuntimeStatus = new OrchestrationRuntimeStatus[]
                {
                    OrchestrationRuntimeStatus.Pending,
                    OrchestrationRuntimeStatus.Running,
                    OrchestrationRuntimeStatus.ContinuedAsNew
                }
            }, CancellationToken.None);

            if (!existingInstancesQueryResult.DurableOrchestrationState.Any())
            {
                // An instance with the specified ID prefix doesn't exist or an existing one stopped running, create one.
                var instanceId = await starter.StartNewAsync<string>(functionName, $"{instanceIdPrefix}_{Guid.NewGuid()}");
                log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
                return starter.CreateCheckStatusResponse(req, instanceId);
            }
            else
            {
                // An instance with the specified ID prefix exists or an existing one still running, don't create one.
                string runningInstances = string.Join(", ", existingInstancesQueryResult.DurableOrchestrationState.Select(dos => $"{dos.InstanceId}"));
                return new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent($"An instance is already running. Details: [{runningInstances}]")
                };
            }
        }
    }
}