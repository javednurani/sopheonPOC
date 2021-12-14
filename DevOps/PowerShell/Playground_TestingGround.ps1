# This is a playground script used for testing methods, functions or concepts for PowerShell scripting. 
# It is in no way to be used in actual build, test, or deploy process for DevOps

$Environment = 'DevOps'
$ResourceGroupValue = "Stratus-$($Environment)";
#$CDNProfileEndpointNameValue = "StratusApp-$($Environment)";

# $CDNHostName = az cdn endpoint show --name $CDNProfileEndpointNameValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
# $CDNHttpsEndpoint = "https://" + $CDNHostName + "/";

$DatabaseName = "d4b5sepl3hpau"

for ($i = 0; $i -lt 100; $i++) {
    Write-Output "Deleting Database: $($DatabaseName)-$($i)"
    az sql db delete --name "$($DatabaseName)-$($i)" --server "stratus-devops-tenantenvironments" --resource-group $ResourceGroupValue --no-wait --yes
}


