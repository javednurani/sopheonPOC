#TODO: Migrate the variables into the master token list and remove this script

[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PathToFile
)

#Environment should always be set from the Pipeline variables. If this isn't set, nothing is getting out
$Environment = $env:Environment;

$TenantName = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');
$JWTClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CJWTClientId" --query value).Replace('"', '');
$MarketingUrl = "https://stratuswebsite$($Environment.ToLower()).z22.web.core.windows.net/";

$fileContent = Get-Content $PathToFile;

$loginName = $TenantName.Replace(".onmicrosoft.com", ".b2clogin.com");

$B2CLoginUrl = "$($loginName)/$($TenantName)";

$fileContent = $fileContent.Replace("&BaseB2CLoginUrl&", $B2CLoginUrl);
$fileContent = $fileContent.Replace("&B2CClientId&", $JWTClientId);
$fileContent = $fileContent.Replace("&BaseMarketingUrl&", $MarketingUrl);

Set-Content -Path $PathToFile -Value $fileContent -Force;