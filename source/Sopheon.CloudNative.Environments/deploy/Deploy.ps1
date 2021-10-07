#Adding blank comments to avoid bad artifact generation

$ResourceGroup = "Stratus-$($env:Environment)";
$FunctionAppStorageAccountName = "stratus$($env:Environment.ToLower())envfuncapp";

$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps";
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');

#region Sql Script Run
Write-Host "Running Sql Server Script for Environment Management";
$FirewallCreation = az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Write-Host "Firewall Rule created";
Write-Host "Running EF Sql Script on Sql Server Database: $($ResourceGroup)...";
Invoke-Sqlcmd -ServerInstance "$($ResourceGroup.ToLower()).database.windows.net" -Database "EnvironmentManagement" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_StratusEnvironmentManagement\EnvironmentManagement\scripts.sql" -QueryTimeout 0;
Write-Host "Complete!";
Write-Host "Removing Firewall Rule";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $ResourceGroup.ToLower();

#upload function app
az functionapp deployment source config-zip --name $ResourceGroup.ToLower() --resource-group $ResourceGroup --src "_StratusEnvironmentManagement\EnvironmentManagement\EnvironmentManagement.zip";

az bicep build --file "$($PSScriptRoot)\ElasticPool_Database_Buffer.bicep";

az storage blob upload --destination 'armTemplates' --destination-path 'ElasticPoolWithBuffer/' --account-name $FunctionAppStorageAccountName --source "$($PSScriptRoot)\ElasticPool_Database_Buffer.json";