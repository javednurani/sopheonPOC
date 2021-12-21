# This is a playground script used for testing methods, functions or concepts for PowerShell scripting. 
# It is in no way to be used in actual build, test, or deploy process for DevOps

$Environment = 'DevOps'
$ResourceGroupValue = "Stratus-$($Environment)";
#$CDNProfileEndpointNameValue = "StratusApp-$($Environment)";

# $CDNHostName = az cdn endpoint show --name $CDNProfileEndpointNameValue --profile-name $ResourceGroupValue --resource-group $ResourceGroupValue --query "hostName" --output tsv;
# $CDNHttpsEndpoint = "https://" + $CDNHostName + "/";

$DatabaseName = "c5ii6vt22nw5i"

for ($i = 0; $i -lt 100; $i++) {
    #Write-Output "Deleting Database: $($DatabaseName)-$($i)"
    #az sql db delete --name "$($DatabaseName)-$($i)" --server "stratus-devops-tenantenvironments" --resource-group $ResourceGroupValue --no-wait --yes
    Write-Output "Running SqlCmd on Database: $($DatabaseName)-$($i)"

    Invoke-Sqlcmd -ServerInstance "stratus-devops-tenantenvironments.database.windows.net" -Database "$($DatabaseName)-$($i)" -Username "sopheon" -Password "M0untains" -Query "CREATE USER jobuser FROM LOGIN jobuser" -QueryTimeout 0
    #-InputFile "..\..\source\Sopheon.CloudNative.Environments\deploy\ElasticJobTarget_CreateJobUser.sql"
}


