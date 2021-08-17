$ClientID = (az keyvault secret show --vault-name "Stratus-Dev" --name "StratusB2CClientId" --query value).Replace('"', '');
$ClientSecret = (az keyvault secret show --vault-name "Stratus-Dev" --name "StratusB2CClientSecret" --query value).Replace('"', '');

$body = @{ grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientID; client_secret = $ClientSecret }

$response = Invoke-RestMethod -Uri https://login.microsoftonline.com/StratusB2CDev.onmicrosoft.com/oauth2/v2.0/token -Method Post -Body $body
$token = $response.access_token

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", 'application/xml')
$headers.Add("Authorization", 'Bearer ' + $token)
$graphuri = 'https://graph.microsoft.com/beta/trustframework/policies'

$response = Invoke-RestMethod -Uri $graphuri -Method Get -Headers $headers
#$response.value 

foreach ($item in $response.value ) {
    Write-Host $item.id;
}

$XMLPath = "$($PSScriptRoot)/../usersignup/azureResources/TrustFrameworkBase.xml"
$xml = [xml](Get-Content $XMLPath);

Write-Output "PolicyId for TrustFrameworkPolicy..."
$xmlPolicyId = $xml.TrustFrameworkPolicy | Select-Object PolicyId;
$id = $xmlPolicyId.PolicyId.ToUpperInvariant();
$id