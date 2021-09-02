# This is a playground script used for testing methods, functions or concepts for PowerShell scripting. 
# It is in no way to be used in actual build, test, or deploy process for DevOps

$ResourceGroup = 'AccCloudTest';
az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Invoke-Sqlcmd -ServerInstance "$($ResourceGroup.ToLower()).database.windows.net" -Database $ResourceGroup -UserName "sopheon" -Password "M0untains" -InputFile "..\..\_Sopheon Environment Management API\drop\migrations\scripts.sql";
Write-Host "Complete!";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $ResourceGroup.ToLower();