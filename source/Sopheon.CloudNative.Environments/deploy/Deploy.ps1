#Adding blank comments to avoid bad artifact generation

$ResourceGroup = "Stratus-$($env:Environment)";
$EnvironmentFunctionAppName = "";

$EnvironmentServerName = "$($ResourceGroup.ToLower())";
$TenantEnvironmentServerName = "$($ResourceGroup.ToLower())-tenantenvironments";
$FunctionAppStorageAccountName = "stratus$($env:Environment.ToLower())funcapp";

$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps";
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');

#region Sql Script Run
Write-Host "Running Sql Server Script for Environment Management";
az sql server firewall-rule create --resource-group $ResourceGroup --server $EnvironmentServerName --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
az sql server firewall-rule create --resource-group $ResourceGroup --server $TenantEnvironmentServerName --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;

Write-Host "Firewall Rules created";

$CredentialsForMasterAndJobScript = Get-Content "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateCredentials.sql" -Raw
$CredentialsForMasterAndJobScript = $CredentialsForMasterAndJobScript.Replace("^Enigma^", $SqlAdminEnigma);
Set-Content -Path $CredentialsForMasterAndJobScript -Force

$CredentialsJobScript = Get-Content "_StratusEnvironmentManagement\EnvironmentManagement\CredentialsJobScript.sql" -Raw
$CredentialsJobScript = $CredentialsJobScript.Replace("^Enigma^", $SqlAdminEnigma);
Set-Content -Path $CredentialsJobScript -Force

Write-Host "Setting Job Agent Credentials on $EnvironmentServerName";
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database 'master' -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateCredentials.sql"

Write-Host "Setting Job Agent Credentials on $TenantEnvironmentServerName";
Invoke-Sqlcmd -ServerInstance "$($TenantEnvironmentServerName).database.windows.net" -Database 'master' -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateCredentials.sql"

Write-Host "Running EF Sql Script on Sql Server Database: $($EnvironmentServerName)...";
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database "EnvironmentManagement" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\scripts.sql" -QueryTimeout 0;

Write-Host "Setting Job User Credentials on $EnvironmentServerName";
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database "TenantEnvironmentTemplate" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateJobUser.sql";

Write-Host "Complete!";

Write-Host "Removing Firewall Rules";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $EnvironmentServerName;
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $TenantEnvironmentServerName;


#upload function app
az functionapp deployment source config-zip --name "$($ResourceGroup.ToLower())-environment" --resource-group $ResourceGroup --src "_StratusEnvironmentManagement\EnvironmentManagement\EnvironmentManagement.zip";

#upload function app
az functionapp deployment source config-zip --name "$($ResourceGroup.ToLower())-resource" --resource-group $ResourceGroup --src "_StratusEnvironmentManagement\EnvironmentManagement\ResourceManagement.zip";


az bicep build --file "$($PSScriptRoot)\ElasticPool_Database_Buffer.bicep";

az storage blob upload --container-name 'armtemplates' --name 'ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json' --account-name $FunctionAppStorageAccountName --file "$($PSScriptRoot)\ElasticPool_Database_Buffer.json";

az bicep build --file "$($PSScriptRoot)\ElasticJobAgent_EFMigration.bicep";

az storage blob upload --container-name 'armtemplates' --name 'ElasticJobAgent/ElasticJobAgent_EFMigration.json' --account-name $FunctionAppStorageAccountName --file "$($PSScriptRoot)\ElasticJobAgent_EFMigration.json";