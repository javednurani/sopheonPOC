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

$CDNTemplate = "$($PSScriptRoot)\CDN_Profile.bicep";
$CDNParametersTemplate = "$($PSScriptRoot)\CDN_Profile_Parameters.json";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^StorageAccountName^', $StorageAccountNameValue)
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$masterParametersContent = Get-Content $MasterParametersTemplate -raw;
$masterParametersContent = $masterParametersContent.Replace('^StorageAccountName^', $StorageAccountNameValue)
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

Write-Host "Enabling Static Website properties...";
# updates a storage account to be a static website setup with auth-mode as login
$StaticWebsiteEnabled = az storage blob service-properties update --account-name $StorageAccountNameValue --static-website --404-document WebApp/index.html --index-document index.html --auth-mode login --query "staticWebsite.enabled";
Write-Host "Static Website enabled: $($StaticWebsiteEnabled) on Storage Account: $($StorageAccountNameValue)";

Write-Host "Setting Static Website url for origin endpoint to CDN";
# Gets the now setup url for the storage account Static Website
# NOTE: This returns the Full HTTPS://*/ url, we need to strip out the /'s and HTTP(S): to be used properly for the CDN Origins
$StorageAccountStaticWebsiteUrl = az storage account show --name $StorageAccountNameValue --resource-group $ResourceGroupValue --query "primaryEndpoints.web" --output tsv;
$CDNProfileEndpointOriginValue = $StorageAccountStaticWebsiteUrl -replace 'https:', '' -replace '/', '' -replace 'http:', '';
Write-Host "Set! Static Website Url: $($CDNProfileEndpointOriginValue)";

#region CDN template
$CDNProfileNameToken = '^CDNProfileName^';
$CDNProfileNameValue = $ResourceGroupValue;
$CDNProfileEndpointNameToken = '^CDNProfileEndpointName^';
$CDNProfileEndpointNameValue = "StratusApp-$($Environment)";
$CDNProfileEndpointMarketingNameValue = "StratusMarketing-$($Environment)";
$CDNProfileEndpointMarketingNameToken = '^CDNProfileEndpointMarketingName^';
$CDNProfileEndpointOriginToken = '^CDNProfileEndpointOrigin^';

Write-Host "Replacing tokens on Master CDN Template...";
$masterCdnTemplate = Get-Content $CDNTemplate -raw;
$masterCdnTemplate = $masterCdnTemplate.Replace($CDNProfileNameToken, $CDNProfileNameValue).Replace($CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue);
$masterCdnTemplate = $masterCdnTemplate.Replace($CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue).Replace($CDNProfileEndpointMarketingNameToken, $CDNProfileEndpointMarketingNameValue);
Set-Content -Value $masterCdnTemplate -Path $CDNTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master CDN Parameters Template...";
$cdnParameters = Get-Content $CDNParametersTemplate -raw;
$cdnParameters = $cdnParameters.Replace($CDNProfileNameToken, $CDNProfileNameValue).Replace($CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue);
$cdnParameters = $cdnParameters.Replace($CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue).Replace($CDNProfileEndpointMarketingNameToken, $CDNProfileEndpointMarketingNameValue);
Set-Content -Value $cdnParameters -Path $CDNParametersTemplate;
Write-Host "Complete!";

Write-Host "Deploying CDN Template to Resource Group: $($ResourceGroup)";
$CDNTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $CDNTemplate --parameters $CDNParametersTemplate --name "$($DeploymentName)-CDN" --query "properties.provisioningState";
Write-Host "CDN Template Deploy: $($CDNTemplateDeploy)";
$CDNHostName = az cdn endpoint show --name $ResourceGroupValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
$CDNHttpsEndpoint = "https://" + $CDNHostName + "/";
Write-Host "CDN Endpoint: $($CDNHttpsEndpoint)";

#endregion

Write-Host "Infrastructure deployment complete!";
