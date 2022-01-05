$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

#Adding blank comments to avoid bad artifact generation
$FunctionAppStorageAccountName = "stratus$($env:Environment.ToLower())funcapp";
$ProductManagementPath = "$($env:System_DefaultWorkingDirectory)\ProductManagementApi";
$ResourceGroup = "Stratus-$($env:Environment)";
$EnvironmentSqlServer = "$($ResourceGroup.ToLower())";
$EnvironmentTenantTemplateDatabase = "TenantEnvironmentTemplate";

$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps";
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');

#region Sql Script Run
Write-Host "Running Sql Server Script for Environment Management";
az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Write-Host "Firewall Rule created";
Write-Host "Running EF Sql Script on Sql Server Database: $($EnvironmentTenantTemplateDatabase)...";
Invoke-Sqlcmd -ServerInstance "$($EnvironmentSqlServer).database.windows.net" -Database "$EnvironmentTenantTemplateDatabase" -UserName "sopheon" -Password $SqlAdminEnigma -InputFile "_ProductManagement\ProductManagement\products_migration.sql" -QueryTimeout 0;
Write-Host "Complete!";
Write-Host "Removing Firewall Rule";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $EnvironmentSqlServer;

$WebApiAppServiceName = "stratus-productmanagement-$env:Environment";

& $ZipUtil "x" "$($PSScriptRoot)/ProductManagement.zip" "-o$($ProductManagementPath)";

& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _ProductManagement\ProductManagement\Product_Management_Configuration.json -f "$ProductManagementPath\*"  -e $env:Environment

& $ZipUtil "a" "-tzip" "$($env:System_DefaultWorkingDirectory)\ProductManagementApi" "$($ProductManagementPath)\*" "-xr!build" "-xr!deploy";


#upload webapp app
az webapp deployment source config-zip --name $WebApiAppServiceName --resource-group $ResourceGroup --src "ProductManagementApi.zip";

az storage blob upload --container-name 'efmigrations' --name 'AppMigrations/products_migration.sql' --account-name $FunctionAppStorageAccountName --file "$($PSScriptRoot)\products_migration.sql";