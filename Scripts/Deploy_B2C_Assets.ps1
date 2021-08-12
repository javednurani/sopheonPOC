[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PolicyId,
    [Parameter(Mandatory = $true)][string]$PathToFile,
    [Parameter(Mandatory = $true)][string]$Environment
)

#TODO: Return this later to complete the TokenReplacer app
# $ClientID = "^B2CClientId^";
# $ClientSecret = "^B2CClientSecret^";
# $TenantId = "^StratusB2CTenantName^";


# Write-Host "Collecting KeyVault secrets for B2C Asset deployments";
$ClientID = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CClientId" --query value).Replace('"', '');
$ClientSecret = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CClientSecret" --query value).Replace('"', '');
$TenantId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');
# $ProxyIdentityFrameworkClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CProxyIdentityFrameworkClientId" --query value).Replace('"', '');
# $B2CIdentityFrameworkClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CIdentityFrameworkClientId" --query value).Replace('"', '');
# $B2CExtensionsObjectId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CExtensionsObjectId" --query value).Replace('"', '');
# $B2CExtensionsClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CExtensionsClientId" --query value).Replace('"', '');
# $B2CPolicyDeploymentMode = $($env:B2CPolicyDeploymentMode);
# $B2CJourneyInsightsDeveloperMode = $($env:B2CJourneyInsightsDeveloperMode);
# $B2CAppInsightsInstrumentationKey = $($env:B2CAppInsightsInstrumentationKey);

try {
    $body = @{ grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientID; client_secret = $ClientSecret }

    $response = Invoke-RestMethod -Uri https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token -Method Post -Body $body
    $token = $response.access_token

    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $headers.Add("Content-Type", 'application/xml')
    $headers.Add("Authorization", 'Bearer ' + $token)

    $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $PolicyId + '/$value'
    $policycontent = Get-Content $PathToFile

    # Write-Host "Updating configurable variables on policy content";
    # # Optional: Change the content of the policy. For example, replace the tenant-name with your tenant name.
    # $policycontent = $policycontent.Replace("nonexistent.onmicrosoft.com", $TenantId)
    # Write-Output "Nonexistent was replaced with: $TenantId"
    # $policycontent = $policycontent.Replace("^ProxyIdentityFrameworkClientId^", $ProxyIdentityFrameworkClientId);
    # $policycontent = $policycontent.Replace("^IdentityFrameworkClientId^", $B2CIdentityFrameworkClientId);
    # $policycontent = $policycontent.Replace("^ExtensionsAppObjectId^", $B2CExtensionsObjectId);
    # $policycontent = $policycontent.Replace("^ExtensionsAppClientId^", $B2CExtensionsClientId);
    # $policycontent = $policycontent.Replace("^B2CPolicyDeploymentMode^", $B2CPolicyDeploymentMode);
    # $policycontent = $policycontent.Replace("^B2CJourneyInsightsDeveloperMode^", $B2CJourneyInsightsDeveloperMode);
    # $policycontent = $policycontent.Replace("^B2CAppInsightsInstrumentationKey^", $B2CAppInsightsInstrumentationKey);
    # Set-Content -Path $PathToFile -Value $policycontent

    $response = Invoke-RestMethod -Uri $graphuri -Method Put -Body $policycontent -Headers $headers
    $response;

    Write-Host "Policy" $PolicyId "uploaded successfully."
}
catch {
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