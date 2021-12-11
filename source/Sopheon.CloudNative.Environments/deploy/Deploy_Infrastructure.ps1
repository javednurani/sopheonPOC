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
$EnvironmentFunctionAppName = "$($ResourceGroupValue.ToLower())-environment";
$FunctionAppStorageAccountName = "stratus$($Environment.ToLower())funcapp"
$AppInsightsName = $ResourceGroupValue;
$EnvironmentSQLServerName = $ResourceGroupValue;
$ElasticJobAgentSQLServerName = "$($ResourceGroupValue)-JobAgent";
$TenantSQLServerName = "$($ResourceGroupValue)-TenantEnvironments";
$EnvironmentManagementSQLServerDatabaseName = "EnvironmentManagement";
$ElasticJobAgentSQLServerDatabaseName = "JobAgent";
$WebServerFarmName = "ASP-$($ResourceGroupValue)-Environment";

$ResourceFunctionAppName = "$($ResourceGroupValue)-resource";
$ResourceWebServerFarmName = "ASP-$($ResourceGroupValue)-Resource";

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentManagementSQLServerName^', $EnvironmentSQLServerName).Replace('^EnvironmentManagementSQLServerDatabaseName^', $EnvironmentManagementSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^ElasticJobAgentSQLServerName^', $ElasticJobAgentSQLServerName).Replace('^ElasticJobAgentSQLServerDatabaseName^', $ElasticJobAgentSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentFunctionAppName^', $EnvironmentFunctionAppName).Replace('^TenantSQLServerName^', $TenantSQLServerName);
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^FunctionStorageAccountName^', $FunctionAppStorageAccountName);
$masterTemplateContent = $masterTemplateContent.Replace('^SqlAdminEngima^', $SqlAdminEnigma).Replace('^EnvironmentWebServerFarmName^', $WebServerFarmName);
$masterTemplateContent = $masterTemplateContent.Replace('^ResourceWebServerFarmName^', $ResourceWebServerFarmName).Replace('^ResourceManagementFunctionAppName^', $ResourceFunctionAppName);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _StratusEnvironmentManagement\EnvironmentManagement\Environments_Configuration.json -f "$PSScriptRoot\*"  -e $Environment

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

$environmentManagementConnectionString = (az sql db show-connection-string --client ado.net --server "$($EnvironmentSQLServerName.ToLower())" --name $EnvironmentManagementSQLServerDatabaseName).Replace('"', '');

$environmentManagementConnectionString = $environmentManagementConnectionString.Replace('<username>', 'sopheon').Replace('<password>', $SqlAdminEnigma);

$connectionString = az webapp config connection-string set --resource-group $ResourceGroupValue --name $EnvironmentFunctionAppName -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;
$connectionString = az webapp config connection-string set --resource-group $ResourceGroupValue --name $ResourceFunctionAppName -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;