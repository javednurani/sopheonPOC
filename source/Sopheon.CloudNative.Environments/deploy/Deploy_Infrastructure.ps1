[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)
# Deploy Azure Resources for release definitions to talk to
$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps"
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";

#Token values
$EnvironmentFunctionAppName = $ResourceGroupValue.ToLower();
$EnvironmentFunctionAppStorageAccountName = "stratus$($Environment.ToLower())envfuncapp"
$AppInsightsName = $ResourceGroupValue;
$EnvironmentSQLServerName = $ResourceGroupValue;
$ElasticJobAgentSQLServerName = "$($ResourceGroupValue)-JobAgent";
$TenantSQLServerName = "$($ResourceGroupValue)-TenantEnvironments";
$EnvironmentManagementSQLServerDatabaseName = "EnvironmentManagement";
$ElasticJobAgentSQLServerDatabaseName = "JobAgent";
$WebServerFarmName = "ASP-$($ResourceGroupValue)";

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentManagementSQLServerName^', $EnvironmentSQLServerName).Replace('^EnvironmentManagementSQLServerDatabaseName^', $EnvironmentManagementSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^ElasticJobAgentSQLServerName^', $ElasticJobAgentSQLServerName).Replace('^ElasticJobAgentSQLServerDatabaseName^', $ElasticJobAgentSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentFunctionAppName^', $EnvironmentFunctionAppName).Replace('^TenantSQLServerName^', $TenantSQLServerName);
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^EnvironmentFunctionStorageAccountName^', $EnvironmentFunctionAppStorageAccountName);
$masterTemplateContent = $masterTemplateContent.Replace('^SqlAdminEngima^', $SqlAdminEnigma).Replace('^WebServerFarmName^', $WebServerFarmName);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" repla
ce -c _StratusEnvironmentManagement\EnvironmentManagement\Environments_Configuration.json -f "$PSScriptRoot\*"  -e $Environment

Write-Host "Deploying Storage Account Template to Resource Group: $($ResourceGroupValue)";
# Creates a deployment for the given resource group and template.json
$GroupExists = az group exists --name $ResourceGroupValue;

if('false' -eq $GroupExists)
{
    Write-Host "Resource Group does not exist; Creating group: $($ResourceGroupValue)";
    az group create --location 'westus' --name $ResourceGroupValue --tags 'Environment Type=Development' 'Owner=Cal' 'Review Date=12-30-21' --query "properties.provisioningState";
}

Write-Host "Deploying Master Template...";
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --name "$($DeploymentName)-MasterDeploy-EnvironmentManagement" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";

$environmentManagementConnectionString = (az sql db show-connection-string --client ado.net --server "$($ResourceGroupValue.ToLower())" --name $SqlServerDatabaseNameValue).Replace('"', '');

$environmentManagementConnectionString = $environmentManagementConnectionString.Replace('<username>', 'sopheon').Replace('<password>', $SqlAdminEnigma);

$connectionString = az webapp config connection-string set --resource-group $ResourceGroupValue --name $ResourceGroupValue.ToLower() -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;