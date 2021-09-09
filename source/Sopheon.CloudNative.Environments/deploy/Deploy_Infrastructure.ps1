
[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$Environment = $env:Environment
)
# Deploy Azure Resources for release definitions to talk to

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";
$SqlServerNameValue = $ResourceGroupValue;
$SqlServerPoolName = "$($ResourceGroupValue)-Pool";
$FunctionAppName = $ResourceGroupValue.ToLower();
$SqlServerDatabaseNameValue = "EnvironmentManagement";
$AppInsightsName = $ResourceGroupValue;

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\Master_Template_Parameters.json";


Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^SqlServerName^', $SqlServerNameValue).Replace('^SqlServerDatabaseName^', $SqlServerDatabaseNameValue)
$masterTemplateContent = $masterTemplateContent.Replace('^SqlElasticPoolName^', $SqlServerPoolName).Replace('^EnvironmentFunctionAppName^', $FunctionAppName)
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$masterParametersContent = Get-Content $MasterParametersTemplate -raw;
$masterParametersContent = $masterParametersContent.Replace('^SqlServerName^', $SqlServerNameValue).Replace('^SqlServerDatabaseName^', $SqlServerDatabaseNameValue);
$masterParametersContent = $masterParametersContent.Replace("^SqlElasticPoolName^", $SqlServerPoolName).Replace('^EnvironmentFunctionAppName^', $FunctionAppName);
$masterParametersContent = $masterParametersContent.Replace('^AppInsightsName^', $AppInsightsName);
Set-Content -Value $masterParametersContent -Path $MasterParametersTemplate;
Write-Host "Complete!";

Write-Host "Deploying Storage Account Template to Resource Group: $($ResourceGroupValue)";
# Creates a deployment for the given resource group and template.json
$GroupExists = az group exists --name $ResourceGroupValue;

if('false' -eq $GroupExists)
{
    Write-Host "Resource Group does not exist; Creating group: $($ResourceGroupValue)";
    az group create --location 'westus' --name $ResourceGroupValue --tags 'Environment Type=Development' 'Owner=Cal' 'Review Date=12-30-21' --query "properties.provisioningState";
}

Write-Host "Deploying Master Template...";
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --parameters $MasterParametersTemplate --name "$($DeploymentName)-MasterDeploy" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";

$SqlAdminEngima = (az keyvault secret show --vault-name "Cloud-DevOps" --name "SqlServerAdminEnigma" --query value).Replace('"', '');

$environmentManagementConnectionString = (az sql db show-connection-string --client ado.net --server "$($ResourceGroupValue.ToLower()).database.windows.net" --name $SqlServerDatabaseNameValue).Replace('"', '');

$environmentManagementConnectionString = $environmentManagementConnectionString.Replace('<username>', 'sopheon').Replace('<password>', $SqlAdminEngima);

az webapp config connection-string set --resource-group $ResourceGroupValue --name $ResourceGroupValue.ToLower() -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;