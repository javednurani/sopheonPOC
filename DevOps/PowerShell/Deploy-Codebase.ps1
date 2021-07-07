Import-Module -Name "$($PSScriptRoot)\Deploy-Config.psm1";

#region Sql Script Run
Write-Host "Running Sql Server Script for Environment Management API";
$FirewallCreation = az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Write-Host "Firewall Rule created";
Write-Host "Running EF Sql Script on Sql Server Database: $($ResourceGroup)...";
Invoke-Sqlcmd -ServerInstance "$($ResourceGroup.ToLower()).database.windows.net" -Database $ResourceGroup -UserName "sopheon" -Password "M0untains" -InputFile "_Sopheon Environment Management API\drop\migrations\scripts.sql";
Write-Host "Complete!";
Write-Host "Removing Firewall Rule";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $ResourceGroup.ToLower();
#endregion

#region SPA Website
Write-Host "Uploading SPA web app to blob storage";
$SpaUploadResults = az storage blob upload-batch -d '$web' --account-name $StorageAccountNameValue -s "Sopheon Full App Deploy";
$SpaUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'$web';
#endregion

#region Web API
Write-Host "Deploy Environment Management API";
$ApiDeployResults = az webapp deployment source config-zip --resource-group $ResourceGroup --name $AppServiceNameValue --src "_Sopheon Environment Management API\drop\Sopheon.CloudNative.Admin.zip" --query "active";
Write-Host "Complete, API Endpoint active: $($ApiDeployResults)";
#endregion