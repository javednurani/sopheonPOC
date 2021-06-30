[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PolicyId,
    [Parameter(Mandatory = $true)][string]$PathToFile,
    [Parameter(Mandatory = $true)][string]$Environment
)

$ClientID = (Get-ChildItem env:StratusB2C$($Environment)ClientId).value;
$ClientSecret = (Get-ChildItem env:StratusB2C$($Environment)).value;
$TenantId = (Get-ChildItem env:StratusB2C$($Environment)TenantId).value;
$ProxyIdentityFrameworkClientId = (Get-ChildItem env:B2C$($Environment)ProxyIdentityFrameworkClientId).value;
$B2CIdentityFrameworkClientId = (Get-ChildItem env:B2C$($Environment)IdentityFrameworkClientId).value;
$B2CExtensionsObjectId = (Get-ChildItem env:B2C$($Environment)ExtensionsObjectId).value;
$B2CExtensionsClientId = (Get-ChildItem env:B2C$($Environment)ExtensionsClientId).value;

try {
    $body = @{grant_type = "client_credentials"; scope = "https://graph.microsoft.com/.default"; client_id = $ClientID; client_secret = $ClientSecret }

    $response = Invoke-RestMethod -Uri https://login.microsoftonline.com/$TenantId/oauth2/v2.0/token -Method Post -Body $body
    $token = $response.access_token

    $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
    $headers.Add("Content-Type", 'application/xml')
    $headers.Add("Authorization", 'Bearer ' + $token)

    $graphuri = 'https://graph.microsoft.com/beta/trustframework/policies/' + $PolicyId + '/$value'
    $policycontent = Get-Content $PathToFile

    # Optional: Change the content of the policy. For example, replace the tenant-name with your tenant name.
    $policycontent = $policycontent.Replace("non-existent.onmicrosoft.com", $TenantId)
    $policycontent = $policycontent.Replace("^ProxyIdentityFrameworkClientId^", $ProxyIdentityFrameworkClientId);
    $policycontent = $policycontent.Replace("^IdentityFrameworkClientId^", $B2CIdentityFrameworkClientId);
    $policycontent = $policycontent.Replace("^ExtensionsAppObjectId^", $B2CExtensionsObjectId);
    $policycontent = $policycontent.Replace("^ExtensionsAppClientId^", $B2CExtensionsClientId);

    $response = Invoke-RestMethod -Uri $graphuri -Method Put -Body $policycontent -Headers $headers

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