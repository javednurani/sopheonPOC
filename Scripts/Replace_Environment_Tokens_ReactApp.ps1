# This script is used at the release pipeline level, as it searches for files .js, based on .ts transcompilations
$Environment = $env:Environment;

$childFiles = dir .\* -include ('*.js', '*.json', '*.ts') -recurse

Write-Host "Running through files: $($childFiles.Length)";

$ShellClientId = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CShellAppClientId" --query value).Replace('"', '');
$TenantName = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusB2CTenantName" --query value).Replace('"', '');
$BrowswerWebAppUrl = (az keyvault secret show --vault-name "Stratus-$($Environment)" --name "StratusBrowserWebAppUrl" --query value).Replace('"', '');
$LoginName = $TenantName.Replace(".onmicrosoft.com", "");


$childFiles | ForEach-Object -Process { 
    Write-Host "START word match on file: $($_.FullName)"
    $rawData = Get-Content -Path $_.FullName -Raw;
    $isDataTouched  = $false;
    
    Write-Host "Looking for Token: '^ShellAppClientId^'";
    if($rawData -match "^ShellAppClientId^") {
        $isDataTouched = $true;
        $rawData = $rawData.Replace("^ShellAppClientId^", $ShellClientId);
        Write-Host "Replaced Shell App Client Id in file: $($_.FullName)";
    }

    Write-Host "Looking for Token: '^B2CTenantName^'";
    if($rawData -match "^B2CTenantName^") {        
        $isDataTouched = $true;
        $rawData = $rawData.Replace("^B2CTenantName^", $LoginName);
        Write-Host "Replaced Tenant Name in file: $($_.FullName)";
    }
    
    Write-Host "Looking for Token: '^BrowserWebAppUrl^'";
    if($rawData -match "^BrowserWebAppUrl^") {        
        $isDataTouched = $true;
        $rawData = $rawData.Replace("^BrowserWebAppUrl^", $BrowswerWebAppUrl);
        Write-Host "Replaced Tenant Name in file: $($_.FullName)";
    }  

    if($isDataTouched) {
        Write-Host "Is data touched, replacing data in file"
         $rawData | Set-Content -Path $_;
    }
    Write-Host "END word match on file: $($_.FullName)"
}