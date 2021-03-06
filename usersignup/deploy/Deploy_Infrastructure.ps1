[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)

# Deploy Azure Resources for release definitions to talk to

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";
$StorageAccountNameValue = "stratus$($Environment.ToLower())b2c";
$B2CLoginName = "https://$((az keyvault secret show --vault-name Stratus-$Environment --name 'StratusB2CTenantLoginName' | ConvertFrom-Json).value).b2clogin.com";


$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\Master_Template_Parameters.json";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^StorageAccountName^', $StorageAccountNameValue).Replace('^B2CLogin^', $B2CLoginName);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$masterParametersContent = Get-Content $MasterParametersTemplate -raw;
$masterParametersContent = $masterParametersContent.Replace('^StorageAccountName^', $StorageAccountNameValue).Replace('^B2CLogin^', $B2CLoginName);
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
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --parameters $MasterParametersTemplate --name "$($DeploymentName)-MasterDeploy-B2CAssets" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";

Write-Host "Enabling Static Website properties...";
# updates a storage account to be a static website setup with auth-mode as login
$StaticWebsiteEnabled = az storage blob service-properties update --account-name $StorageAccountNameValue --static-website --404-document TermsOfService/index.html --index-document index.html --auth-mode login --query "staticWebsite.enabled";
Write-Host "Static Website enabled: $($StaticWebsiteEnabled) on Storage Account: $($StorageAccountNameValue)";

Write-Host "Infrastructure deployment complete!";

#endregion
