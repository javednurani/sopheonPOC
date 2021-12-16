#Adding blank comments to avoid bad artifact generation

$ResourceGroup = "Stratus-$($env:Environment)";
$EnvironmentFunctionAppName = "";

$EnvironmentServerName = "$($ResourceGroup.ToLower())-environment";
$TenantEnvironmentServerName = "$($ResourceGroup.ToLower())-tenantenvironments"
$FunctionAppStorageAccountName = "stratus$($env:Environment.ToLower())funcapp";

$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps";
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');

#region Sql Script Run
Write-Host "Running Sql Server Script for Environment Management";
az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Write-Host "Firewall Rule created";
Write-Host "Running EF Sql Script on Sql Server Database: $($ResourceGroup)...";
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database 'master' -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateCredentials.sql"
Invoke-Sqlcmd -ServerInstance "$($TenantEnvironmentServerName).database.windows.net" -Database 'master' -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateCredentials.sql"
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database "EnvironmentManagement" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\scripts.sql" -QueryTimeout 0;
Invoke-Sqlcmd -ServerInstance "$($EnvironmentServerName).database.windows.net" -Database "TenantEnvironmentTemplate" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\ElasticJobTarget_CreateJobUser.sql";

Write-Host "Complete!";
Write-Host "Removing Firewall Rule";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $ResourceGroup.ToLower();

#upload function app
az functionapp deployment source config-zip --name "$($ResourceGroup.ToLower())-environment" --resource-group $ResourceGroup --src "_StratusEnvironmentManagement\EnvironmentManagement\EnvironmentManagement.zip";

#upload function app
az functionapp deployment source config-zip --name "$($ResourceGroup.ToLower())-resource" --resource-group $ResourceGroup --src "_StratusEnvironmentManagement\EnvironmentManagement\ResourceManagement.zip";


az bicep build --file "$($PSScriptRoot)\ElasticPool_Database_Buffer.bicep";

az storage blob upload --container-name 'armtemplates' --name 'ElasticPoolWithBuffer/ElasticPool_Database_Buffer.json' --account-name $FunctionAppStorageAccountName --file "$($PSScriptRoot)\ElasticPool_Database_Buffer.json";

az bicep build --file "$($PSScriptRoot)\ElasticJobAgent_EFMigration.bicep";

az storage blob upload --container-name 'armtemplates' --name 'ElasticJobAgent/ElasticJobAgent_EFMigration.json' --account-name $FunctionAppStorageAccountName --file "$($PSScriptRoot)\ElasticJobAgent_EFMigration.json";