[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PathToFile
)

#Environment should always be set from the Pipeline variables. If this isn't set, nothing is getting out
$Environment = $env:Environment;

$TenantName = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');
$BrowswerWebAppUrl = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusBrowserWebAppUrl" --query value).Replace('"', '');
$ShellAppClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CShellAppClientId" --query value).Replace('"', '');

$fileContent = Get-Content $PathToFile;

$loginName = $TenantName.Replace(".onmicrosoft.com", "");

$fileContent = $fileContent.Replace("nonexistent.onmicrosoft.com", $TenantName);
$fileContent = $fileContent.Replace("^B2CLoginName^", $loginName);
$fileContent = $fileContent.Replace("^B2CClientId^", $ShellAppClientId);
$fileContent = $fileContent.Replace("&BrowserWebAppUrl&", $BrowswerWebAppUrl);

Set-Content -Path $PathToFile -Value $fileContent -Force;