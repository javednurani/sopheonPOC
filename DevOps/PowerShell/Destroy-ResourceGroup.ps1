Import-Module -Name "$($PSScriptRoot)\Deploy-Config.psm1";


az group delete --name $($ResourceGroup) --yes