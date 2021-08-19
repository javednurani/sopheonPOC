[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PathToFolder,
    [Parameter(Mandatory = $true)][string]$Environment
)

# Write-Host "Collecting KeyVault secrets for B2C Asset deployments";
$ClientID = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CClientId" --query value).Replace('"', '');
$ClientSecret = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CClientSecret" --query value).Replace('"', '');
$TenantId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');

try 
{
    $body = @{ grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientID; client_secret = $ClientSecret };

    $response = Invoke-RestMethod -Uri https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token -Method Post -Body $body;
    $token = $response.access_token;

    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]";
    $headers.Add("Content-Type", 'application/xml');
    $headers.Add("Authorization", 'Bearer ' + $token);

    $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies';

    # Get all the PolicyIds on the B2C Tenant
    $response = Invoke-RestMethod -Uri $graphuri -Method Get -Headers $headers;

    # Get all .xml policy files in folder...
    $sourceFiles = Get-ChildItem -Path $PathToFolder | Where-Object { $_.Name.EndsWith(".xml") };
    $sourceIds = @();    
    $sourceFiles.ForEach({
        $XMLPath = $_.FullName;
        $xml = [xml](Get-Content $XMLPath);
        $xmlPolicyId = $xml.TrustFrameworkPolicy | Select-Object PolicyId;
        $sourceIds += $xmlPolicyId.PolicyId.ToUpper();
    });

    # Loop through and see if any of the B2C Ids, match with the source files Id. If not, then mark for removal from B2C Tenant
    $policiesMarkedForDeletion = @();
    foreach ($item in $response.value ) {        
        if($sourceIds.Contains($item.id)) {
            continue;
        } else {
            $policiesMarkedForDeletion += $item.id;
        }
    }

    # Loop through the source files and upload them to Azure AD B2C
    foreach($file in $sourceFiles) {
        $xml = [xml](Get-Content $file.FullName);
        $xmlPolicyId = $xml.TrustFrameworkPolicy | Select-Object PolicyId;
        $policyId = $xmlPolicyId.PolicyId.ToUpper();
        $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $policyId + '/$value';

        Write-Host 'Uploading File: ' $file.Name '; For PolicyId: ' $policyId;
        $policycontent = Get-Content $file.FullName;
        Invoke-RestMethod -Uri $graphuri -Method Put -Body $policycontent -Headers $headers;

        Write-Host "Policy $policyId uploaded successfully.";
        Write-Host '';

    }

    if($policiesMarkedForDeletion.Length -gt 0) {
        # Loop through the marked for deletion array
        foreach($deleteId in $policiesMarkedForDeletion) {
            Write-Host "Deleting PolicyId: $deleteId";
            $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $deleteId;

            Invoke-RestMethod -Uri $graphuri -Method Delete -Headers $headers;
            Write-Host "Policy $deleteId was successfully deleted.";
            Write-Host '';

        }    
    }

    Write-Host "...COMPLETE: Uploading B2C Custom Policies via GraphAPI...";
}
catch 
{
    Write-Host "StatusCode:" $_.Exception.Response.StatusCode.value__

    $_

    $streamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
    $streamReader.BaseStream.Position = 0
    $streamReader.DiscardBufferedData()
    $errResp = $streamReader.ReadToEnd()
    $streamReader.Close()

    $ErrResp

    exit 1
}

exit 0