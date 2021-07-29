$body = @{ grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = "0a6d2acf-a564-44fe-b36f-f25137373cf1"; client_secret = "MX6nMi-s4g-__mHr.ES9SV4l3Wg7d692sn" }

$response = Invoke-RestMethod -Uri https://login.microsoftonline.com/5767b0c7-d26a-4724-8f99-35323734361b/oauth2/v2.0/token -Method Post -Body $body
$token = $response.access_token

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", 'application/xml')
$headers.Add("Authorization", 'Bearer ' + $token)


$Restresponse = Invoke-RestMethod -Method Put -Uri "https://management.azure.com/subscriptions/1c4bef1d-8a40-4a6d-96d6-764bb466ac46/resourceGroups/Stratus-Dev/providers/Microsoft.AzureActiveDirectory/b2cDirectories/hueber.onmicrosoft.com?api-version=2019-01-01-preview" -Headers $headers
$Restresponse;