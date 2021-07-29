$ResourceGroup = 'AccCloudTest';
az sql server firewall-rule create --resource-group $ResourceGroup --server $ResourceGroup.ToLower() --name DeployMachine --start-ip-address 50.200.9.230 --end-ip-address 50.200.9.230;
Invoke-Sqlcmd -ServerInstance "$($ResourceGroup.ToLower()).database.windows.net" -Database $ResourceGroup -UserName "sopheon" -Password "M0untains" -InputFile "..\..\_Sopheon Environment Management API\drop\migrations\scripts.sql";
Write-Host "Complete!";
az sql server firewall-rule delete --name DeployMachine --resource-group $ResourceGroup --server $ResourceGroup.ToLower();