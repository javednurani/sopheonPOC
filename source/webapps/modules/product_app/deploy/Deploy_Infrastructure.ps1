[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)

# Deploy Azure Resources for release definitions to talk to

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";
$StorageAccountNameValue = "stratuswebsite$($Environment.ToLower())";

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\Master_Template_Parameters.json";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^StorageAccountName^', $StorageAccountNameValue);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$masterParametersContent = Get-Content $MasterParametersTemplate -raw;
$masterParametersContent = $masterParametersContent.Replace('^StorageAccountName^', $StorageAccountNameValue);
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
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --parameters $MasterParametersTemplate --name "$($DeploymentName)-MasterDeploy-AppProducts" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";

#!!!TODO: The storage account should already be in place from the Shell instance. We need to re-evaluate this usage later on when we refactor bicep pattern
#
# Write-Host "Enabling Static Website properties...";
# # updates a storage account to be a static website setup with auth-mode as login
# $StaticWebsiteEnabled = az storage blob service-properties update --account-name $StorageAccountNameValue --static-website --404-document index.html --index-document index.html --auth-mode login --query "staticWebsite.enabled";
# Write-Host "Static Website enabled: $($StaticWebsiteEnabled) on Storage Account: $($StorageAccountNameValue)";

Write-Host "Setting Static Website url for origin endpoint to CDN";
# Gets the now setup url for the storage account Static Website
# NOTE: This returns the Full HTTPS://*/ url, we need to strip out the /'s and HTTP(S): to be used properly for the CDN Origins
$StorageAccountStaticWebsiteUrl = az storage account show --name $StorageAccountNameValue --resource-group $ResourceGroupValue --query "primaryEndpoints.web" --output tsv;
$CDNProfileEndpointOriginValue = $StorageAccountStaticWebsiteUrl -replace 'https:', '' -replace '/', '' -replace 'http:', '';
Write-Host "Set! Static Website Url: $($CDNProfileEndpointOriginValue)";


Write-Host "Infrastructure deployment complete!";
