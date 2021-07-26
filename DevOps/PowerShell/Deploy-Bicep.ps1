Import-Module -Name "$($PSScriptRoot)\Deploy-Config.psm1";

$DeploymentName = "ADO-Deployment";

$MasterTemplate = "$($PSScriptRoot)\..\Bicep\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\..\Bicep\Master_Template_Parameters.json";

#region Master template
Write-Host "Replacing tokens on Master Template...";
$a = Get-Content $MasterTemplate -raw;
$a = $a -replace $StorageAccountNameToken, $StorageAccountNameValue -replace $KeyVaultNameToken, $KeyVaultNameValue
$a = $a -replace $ResourceGroupToken, $ResourceGroupValue;
Set-Content -Value $a -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$a = Get-Content $MasterParametersTemplate -raw;
$a = $a -replace $StorageAccountNameToken, $StorageAccountNameValue -replace $ResourceGroupToken, $ResourceGroupValue;
$a = $a -replace $ResourceGroupToken, $ResourceGroupValue;
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