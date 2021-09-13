# This is a playground script used for testing methods, functions or concepts for PowerShell scripting. 
# It is in no way to be used in actual build, test, or deploy process for DevOps

$Environment = 'Preview'
$ResourceGroupValue = "Stratus-$($Environment)";
$CDNProfileEndpointNameValue = "StratusApp-$($Environment)";

$CDNHostName = az cdn endpoint show --name $CDNProfileEndpointNameValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
$CDNHttpsEndpoint = "https://" + $CDNHostName + "/";
Write-Host $CDNHttpsEndpoint