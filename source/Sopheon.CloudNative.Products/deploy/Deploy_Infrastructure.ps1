
[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)
# Deploy Azure Resources for release definitions to talk to
# $azureKeyVault = "Cloud-DevOps";
# if ($env:AzureEnvironment -eq "Prod"){
#     $azureKeyVault = "Prod-Cloud-DevOps"
# }

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";
$WebApiAppServiceName = $ResourceGroupValue.ToLower();
$WebApiAppStorageAccountName = "stratus$($Environment.ToLower())webapiapp"
$AppInsightsName = $ResourceGroupValue;

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\Master_Template_Parameters.json";


Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $masterTemplateContent -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^WebApiProductsAppName^', $WebApiAppServiceName);
$masterTemplateContent = $masterTemplateContent.Replace('^WebApiProductsStorageAccountName^', $WebApiAppStorageAccountName).Replace('^Environment^', $Environment);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$masterParametersContent = Get-Content $MasterParametersTemplate -raw;
$masterParametersContent = $masterParametersContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^WebApiProductsAppName^', $WebApiAppServiceName);
$masterParametersContent = $masterParametersContent.Replace('^WebApiProductsStorageAccountName^', $WebApiAppStorageAccountName).Replace('^Environment^', $Environment);
Set-Content -Value $masterParametersContent -Path $MasterParametersTemplate;
Write-Host "Complete!";

# TODO: Add in tokens for later configuration
#& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _ProductManagement\ProductManagement\Product_Management_Configuration.json -f "$PSScriptRoot\*"  -e $Environment

Write-Host "Deploying Storage Account Template to Resource Group: $($ResourceGroupValue)";
# Creates a deployment for the given resource group and template.json
$GroupExists = az group exists --name $ResourceGroupValue;

if('false' -eq $GroupExists)
{
    Write-Host "Resource Group does not exist; Creating group: $($ResourceGroupValue)";
    az group create --location 'westus' --name $ResourceGroupValue --tags 'Environment Type=Development' 'Owner=Cal' 'Review Date=6-30-22' --query "properties.provisioningState";
}

Write-Host "Deploying Master Template...";
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --parameters $MasterParametersTemplate --name "$($DeploymentName)-MasterDeploy-ProductManagement" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";
