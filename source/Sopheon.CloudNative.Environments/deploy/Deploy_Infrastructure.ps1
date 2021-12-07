
[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)
# Deploy Azure Resources for release definitions to talk to
$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod"){
    $azureKeyVault = "Prod-Cloud-DevOps"
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');


$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";
$WebServerFarmName = "ASP-$($ResourceGroupValue)";
$SqlServerNameValue = $ResourceGroupValue;
$SqlServerPoolName = "$($ResourceGroupValue)-Pool";
$FunctionAppName = $ResourceGroupValue.ToLower();
$SqlServerDatabaseNameValue = "EnvironmentManagement";
$FunctionAppStorageAccountName = "stratus$($Environment.ToLower())envfuncapp"
$AppInsightsName = $ResourceGroupValue;

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";


Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^SqlServerName^', $SqlServerNameValue).Replace('^SqlServerDatabaseName^', $SqlServerDatabaseNameValue);
$masterTemplateContent = $masterTemplateContent.Replace('^SqlElasticPoolName^', $SqlServerPoolName).Replace('^EnvironmentFunctionAppName^', $FunctionAppName);
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^EnvironmentFunctionStorageAccountName^', $FunctionAppStorageAccountName);
$masterTemplateContent = $masterTemplateContent.Replace('^SqlAdminEngima^', $SqlAdminEnigma).Replace('^WebServerFarmName^', $WebServerFarmName);
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

$environmentManagementConnectionString = (az sql db show-connection-string --client ado.net --server "$($ResourceGroupValue.ToLower())" --name $SqlServerDatabaseNameValue).Replace('"', '');

$environmentManagementConnectionString = $environmentManagementConnectionString.Replace('<username>', 'sopheon').Replace('<password>', $SqlAdminEnigma);

$connectionString = az webapp config connection-string set --resource-group $ResourceGroupValue --name $ResourceGroupValue.ToLower() -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;