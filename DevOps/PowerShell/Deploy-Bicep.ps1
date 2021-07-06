Import-Module -Name "$($PSScriptRoot)\Deploy-Config.psm1";

$DeploymentName = "ADO-Deployment";

$MasterTemplate = "$($PSScriptRoot)\..\Bicep\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\..\Bicep\Master_Template_Parameters.json";

$CDNTemplate = "$($PSScriptRoot)\..\Bicep\CDN_Template.bicep";
$CDNParametersTemplate = "$($PSScriptRoot)\..\Bicep\CDN_Template_Parameters.json";

#region Master template
Write-Host "Replacing tokens on Master Template...";
$a = Get-Content $MasterTemplate -raw;
$a = $a -replace $StorageAccountNameToken, $StorageAccountNameValue -replace $ResourceGroupToken, $ResourceGroupValue;
$a = $a -replace $AppServiceNameToken, $AppServiceNameValue -replace $ResourceGroupToken, $ResourceGroupValue;
$a = $a -replace $SqlServerNameToken, $SqlServerNameValue -replace $SqlServerElasticPoolToken, $SqlServerElasticPoolValue;
$a = $a -replace $SqlServerDatabaseNameToken, $SqlServerDatabaseNameValue -replace $AppInsightsSpaNameToken, $AppInsightsSpaNameValue;
Set-Content -Value $a -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$a = Get-Content $MasterParametersTemplate -raw;
$a = $a -replace $StorageAccountNameToken, $StorageAccountNameValue -replace $ResourceGroupToken, $ResourceGroupValue;
$a = $a -replace $AppServiceNameToken, $AppServiceNameValue -replace $ResourceGroupToken, $ResourceGroupValue;
$a = $a -replace $SqlServerNameToken, $SqlServerNameValue -replace $SqlServerElasticPoolToken, $SqlServerElasticPoolValue;
$a = $a -replace $SqlServerDatabaseNameToken, $SqlServerDatabaseNameValue -replace $SqlServerAuditStorageUrlToken, $SqlServerAuditStorageUrlValue;
$a = $a -replace $AppInsightsSpaNameToken, $AppInsightsSpaNameValue;
Set-Content -Value $a -Path $MasterParametersTemplate;
Write-Host "Complete!";

Write-Host "Deploying Storage Account Template to Resource Group: $($ResourceGroup)";
# Creates a deployment for the given resource group and template.json

$GroupExists = az group exists --name $ResourceGroupValue;

if('false' -eq $GroupExists)
{
    Write-Host "Resource Group does not exist; Creating group: $($ResourceGroupValue)";
    az group create --location 'westus2' --name $ResourceGroupValue --tags 'Environment Type=Research' 'Owner=CloudTeam-1' 'Review Date=06-30-21' --query "properties.provisioningState";
}

Write-Host "Deploying Master Template...";
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroup --template-file $MasterTemplate --parameters $MasterParametersTemplate --name "$($DeploymentName)-MasterDeploy" --query "properties.provisioningState";
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";


Write-Host "Enabling Static Website properties...";
# updates a storage account to be a static website setup with auth-mode as login
$StaticWebsiteEnabled = az storage blob service-properties update --account-name $StorageAccountNameValue --static-website --404-document index.html --index-document index.html --auth-mode login --query "staticWebsite.enabled";
Write-Host "Static Website enabled: $($StaticWebsiteEnabled) on Storage Account: $($StorageAccountNameValue)";
#endregion

Write-Host "Setting Static Website url for origin endpoint to CDN";
# Gets the now setup url for the storage account Static Website
# NOTE: This returns the Full HTTPS://*/ url, we need to strip out the /'s and HTTP(S): to be used properly for the CDN Origins
$StorageAccountStaticWebsiteUrl = az storage account show --name $ResourceGroup.ToLower() --resource-group $ResourceGroup --query "primaryEndpoints.web" --output tsv;
$CDNProfileEndpointOriginValue = $StorageAccountStaticWebsiteUrl -replace 'https:', '' -replace '/', '' -replace 'http:', '';
Write-Host "Set! Static Website Url: $($CDNProfileEndpointOriginValue)";

#endregion

#region CDN template
Write-Host "Replacing tokens on Master CDN Template...";
$a = Get-Content $CDNTemplate -raw;
$a = $a -replace $CDNProfileNameToken, $CDNProfileNameValue -replace $CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue;
$a = $a -replace $CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue -replace $ResourceGroupToken, $ResourceGroupValue;
Set-Content -Value $a -Path $CDNTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master CDN Parameters Template...";
$a = Get-Content $CDNParametersTemplate -raw;
$a = $a -replace $CDNProfileNameToken, $CDNProfileNameValue -replace $CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue;
$a = $a -replace $CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue -replace $ResourceGroupToken, $ResourceGroupValue;
Set-Content -Value $a -Path $CDNParametersTemplate;
Write-Host "Complete!";

Write-Host "Deploying CDN Template to Resource Group: $($ResourceGroup)";
$CDNTemplateDeploy = az deployment group create --resource-group $ResourceGroup --template-file $CDNTemplate --parameters $CDNParametersTemplate --name "$($DeploymentName)-CDN" --query "properties.provisioningState";
Write-Host "CDN Template Deploy: $($CDNTemplateDeploy)";
$CDNHostName = az cdn endpoint show --name $ResourceGroupValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
$CDNHttpsEndpoint = "https://" + $CDNHostName + "/";
Write-Host "CDN Endpoint: $($CDNHttpsEndpoint)";

#endregion