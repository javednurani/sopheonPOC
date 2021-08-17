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

    $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies'

    $response = Invoke-RestMethod -Uri $graphuri -Method Get -Headers $headers
    $sourceFiles = Get-ChildItem -Path $PathToFolder | Where-Object { $_.Name.EndsWith(".xml") };
    $sourceIds = $sourceFiles | Select-Object {        
        $XMLPath = $_.FullName;
        $xml = [xml](Get-Content $XMLPath);
        $xmlPolicyId = $xml.TrustFrameworkPolicy | Select-Object PolicyId; 
        return $xmlPolicyId.PolicyId;        
    };


    $policiesMarkedForDeletion = @();
    foreach ($item in $response.value ) {
        Write-Host $item.id;
        $sourceFiles.ForEach({
            $XMLPath = $_.FullName;
            $xml = [xml](Get-Content $XMLPath);
            $xmlPolicyId = $xml.TrustFrameworkPolicy | Select-Object PolicyId;
            if($item.id -ne $xmlPolicyId.PolicyId.ToUpperInvariant()) {
                $policiesMarkedForDeletion += $item.id;
            }   
        });    
    }

    # $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $PolicyId + '/$value';
    # $policycontent = Get-Content $PathToFile;

    # Invoke-RestMethod -Uri $graphuri -Method Put -Body $policycontent -Headers $headers

    Write-Host "Policy" $PolicyId "uploaded successfully."
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