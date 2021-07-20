[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$EnvironmentName,
    [Parameter(Mandatory = $false)][string]$KeyVaultToCopy = "Dev",
    [Parameter(Mandatory = $false)][string]$AdoProjectName = "Stratus"#$(System.TeamProject)
)

# Deploy Azure Resources for release definitions to talk to

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($EnvironmentName)";
$StorageAccountNameValue = "stratuswebsite$($EnvironmentName.ToLower())";
$KeyVaultNameValue = $ResourceGroupValue;

$MasterTemplate = "$($PSScriptRoot)\..\DevOps\Bicep\Master_Template.bicep";
$MasterParametersTemplate = "$($PSScriptRoot)\..\DevOps\Bicep\Master_Template_Parameters.json";

$CDNTemplate = "$($PSScriptRoot)\..\DevOps\Bicep\CDN_Profile.bicep";
$CDNParametersTemplate = "$($PSScriptRoot)\..\DevOps\Bicep\CDN_Profile_Parameters.json";

Write-Host "Replacing tokens on Master Template...";
$a = Get-Content $MasterTemplate -raw;
$a = $a -replace '&StorageAccountName&', $StorageAccountNameValue -replace '&KeyVaultName&', $KeyVaultNameValue
Set-Content -Value $a -Path $MasterTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master Parameters Template...";
$a = Get-Content $MasterParametersTemplate -raw;
$a = $a -replace '&StorageAccountName&', $StorageAccountNameValue -replace '&KeyVaultName&', $KeyVaultNameValue;
Set-Content -Value $a -Path $MasterParametersTemplate;
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
$StaticWebsiteEnabled = az storage blob service-properties update --account-name $StorageAccountNameValue --static-website --404-document index.html --index-document index.html --auth-mode login --query "staticWebsite.enabled";
Write-Host "Static Website enabled: $($StaticWebsiteEnabled) on Storage Account: $($StorageAccountNameValue)";

Write-Host "Setting Static Website url for origin endpoint to CDN";
# Gets the now setup url for the storage account Static Website
# NOTE: This returns the Full HTTPS://*/ url, we need to strip out the /'s and HTTP(S): to be used properly for the CDN Origins
$StorageAccountStaticWebsiteUrl = az storage account show --name $ResourceGroupValue.ToLower() --resource-group $ResourceGroupValue --query "primaryEndpoints.web" --output tsv;
$CDNProfileEndpointOriginValue = $StorageAccountStaticWebsiteUrl -replace 'https:', '' -replace '/', '' -replace 'http:', '';
Write-Host "Set! Static Website Url: $($CDNProfileEndpointOriginValue)";

#region CDN template
$CDNProfileNameToken = '&CDNProfileName&';
$CDNProfileNameValue = $ResourceGroupValue;
$CDNProfileEndpointNameToken = '&CDNProfileEndpointName&';
$CDNProfileEndpointNameValue = "StratusApp-$($EnvironmentName)";
$CDNProfileEndpointMarketingNameValue = "StratusMarketing-$($EnvironmentName)";
$CDNProfileEndpointMarketingNameToken = '&CDNProfileEndpointMarketingName&';
$CDNProfileEndpointOriginToken = '&CDNProfileEndpointOrigin&';

Write-Host "Replacing tokens on Master CDN Template...";
$a = Get-Content $CDNTemplate -raw;
$a = $a -replace $CDNProfileNameToken, $CDNProfileNameValue -replace $CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue;
$a = $a -replace $CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue -replace $CDNProfileEndpointMarketingNameToken, $CDNProfileEndpointMarketingNameValue;
Set-Content -Value $a -Path $CDNTemplate;
Write-Host "Complete!";

Write-Host "Replacing tokens on Master CDN Parameters Template...";
$a = Get-Content $CDNParametersTemplate -raw;
$a = $a -replace $CDNProfileNameToken, $CDNProfileNameValue -replace $CDNProfileEndpointNameToken, $CDNProfileEndpointNameValue;
$a = $a -replace $CDNProfileEndpointOriginToken, $CDNProfileEndpointOriginValue -replace $CDNProfileEndpointMarketingNameToken, $CDNProfileEndpointMarketingNameValue;
Set-Content -Value $a -Path $CDNParametersTemplate;
Write-Host "Complete!";

Write-Host "Deploying CDN Template to Resource Group: $($ResourceGroup)";
$CDNTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $CDNTemplate --parameters $CDNParametersTemplate --name "$($DeploymentName)-CDN" --query "properties.provisioningState";
Write-Host "CDN Template Deploy: $($CDNTemplateDeploy)";
$CDNHostName = az cdn endpoint show --name $ResourceGroupValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
$CDNHttpsEndpoint = "https://" + $CDNHostName + "/";
Write-Host "CDN Endpoint: $($CDNHttpsEndpoint)";

#endregion

# Migrate KeyVault secrets to new KeyVault for EnvironmentName
$secretNames = az keyvault secret list --vault-name Stratus-$KeyVaultToCopy | ConvertFrom-Json

$secretNames.foreach{
    $value = (az keyvault secret show --vault-name Stratus-$KeyVaultToCopy --name $_.name | ConvertFrom-Json).value;
    az keyvault secret set --name $_.name --vault-name $KeyVaultNameValue --value $value    
}

$tfsApiUri = "https://zeus.sopheon.com/DefaultCollection/$AdoProjectName/_apis";
$tfsReleaseDefinitionUri = $tfsApiUri + "/Release/definitions/";
$tfsReleaseFolderUri = $tfsApiUri + "/Release/folders/";
$tfsVersion = "?api-version=6.0-preview";
$AzureDevOpsPAT = "nvxevxp2salzicdc66f635irfzo45gonaefa43v5jj2myrv3kbta";
$AzureDevOpsAuthenicationHeader = @{Authorization = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes(":$($AzureDevOpsPAT)")) }

# Create the new folder path for the rest of the release definitions to use
$tfsCreateFolderPath = $tfsReleaseFolderUri + $tfsVersion;

$jsonToSend = @"
{
    "createdBy": "id",
    "createdOn": "",
    "description": "",
    "lastChangedBy": "",
    "lastChangedDate": "",
    "path": "\\$EnvironmentName"
}
"@;
# Create Folder RESTful API
$CreatedFolder = Invoke-RestMethod -Uri $tfsCreateFolderPath -Body $jsonToSend -Headers $AzureDevOpsAuthenicationHeader -ContentType "application/json" -Method 'Post' -Verbose -Debug;


# Create and assign variables on Variable Group
$variableGroupsByIdUri = "/distributedtask/variablegroups/12";

$tfsGetVariableGroupsUri = $tfsApiUri + $variableGroupsByIdUri + $tfsVersion;

$variableGroupToClone = Invoke-RestMethod -Uri $tfsGetVariableGroupsUri -Headers $AzureDevOpsAuthenicationHeader -Method 'Get' -Verbose -Debug;

$variableGroupToClone.name = $EnvironmentName;
$variableGroupToClone.createdBy = $null;
$variableGroupToClone.createdOn = $null;
$variableGroupToClone.modifiedBy = $null;
$variableGroupToClone.modifiedOn = $null;
$variableGroupToClone.variables.Environment.value = $EnvironmentName;
$variableGroupToClone.variables.StorageAccount.value = $StorageAccountNameValue;
$variableGroupToClone.variableGroupProjectReferences[0].name = $EnvironmentName;

$tfsCreateVariableGroup = "/distributedtask/variablegroups";

$tfsCreateVariableGroupUri = $tfsApiUri + $tfsCreateVariableGroup + $tfsVersion;

$json = @($variableGroupToClone) | ConvertTo-Json -Depth 99;
$variableGroupCreated = Invoke-RestMethod -Uri $tfsCreateVariableGroupUri -Headers $AzureDevOpsAuthenicationHeader -Method 'Post' -Body $json -ContentType "application/json" -Verbose -Debug;


# Get Release Definition
$tfsGetListDefinitionUri = $tfsReleaseDefinitionUri + $tfsVersion;
Write-Host "URL: $tfsReleaseDefinitionUri"
$Release = Invoke-RestMethod -Uri $tfsGetListDefinitionUri -Headers $AzureDevOpsAuthenicationHeader

# Get the B2C Assets

$releasesToClone = $Release.value | Where-Object { $_.name.StartsWith("Templates -") }

$releasesToClone | ForEach-Object {
    $tfsGetDefinitionByIdUri = $tfsReleaseDefinitionUri + $_.id + $tfsVersion;
    $definedRelease = Invoke-RestMethod -Uri $tfsGetDefinitionByIdUri -Headers $AzureDevOpsAuthenicationHeader

    $definedRelease.createdBy = $null;
    $definedRelease.createdOn = $null;
    $definedRelease.modifiedBy = $null;
    $definedRelease.modifiedOn = $null;
    $definedRelease.environments[0].currentRelease = $null;
    $definedRelease.environments[0].badgeUrl = $null;
    $definedRelease.url = $null;
    $definedRelease._links = $null;
    $definedRelease.id = $null;
    $definedRelease.name = $definedRelease.name.Replace("Templates", $EnvironmentName);
    $definedRelease.path = $CreatedFolder.path;  
    $definedRelease.variableGroups = @($variableGroupCreated.id);

    $definedRelease.releaseNameFormat = $definedRelease.releaseNameFormat.Replace("Templates", $EnvironmentName)
    
    $tfsCreateReleaseDefinition = $tfsReleaseDefinitionUri + $tfsVersion;
    $json = @($definedRelease) | ConvertTo-Json -Depth 99;
    $createdRelease = Invoke-RestMethod -Uri $tfsCreateReleaseDefinition -Method 'Post' -Headers $AzureDevOpsAuthenicationHeader -Body $json -ContentType "application/json";    
}
