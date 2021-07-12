[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PathToFile
)

#Environment should always be set from the Pipeline variables. If this isn't set, nothing is getting out
$Environment = $env:Environment;

$TenantName = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');
$JWTClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CJWTClientId" --query value).Replace('"', '');
$ShellClientId = (az keyvault secret show --vault-name "Stratus$($Envrionment)" --name "StratusB2CShellAppClientId" --query value).Replace('"', '');

$fileContent = Get-Content $PathToFile;

$loginName = $TenantName.Replace(".onmicrosoft.com", "");

$fileContent = $fileContent.Replace("non-existent.onmicrosoft.com", $TenantName);
$fileContent = $fileContent.Replace("^B2CLoginName^", $loginName);
$fileContent = $fileContent.Replace("^B2CClientId^", $JWTClientId);
$fileContent = $fileContent.Replace("&ShellAppClientId&", $ShellClientId);
$fileContent = $fileContent.Replace("&B2CTenantName&", $loginName);

Set-Content -Path $PathToFile -Value $fileContent -Force;