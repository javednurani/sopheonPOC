# This script is used at the release pipeline level, as it searches for files .js, based on .ts transcompilations

$childFiles = dir .\* -include ('*.js', '*.json') -recurse

Write-Host "Running through files: $($childFiles.Length)";

$childFiles | ForEach-Object -Process { 
    Write-Host "START word match on file: $($_.FullName)"
    $rawData = Get-Content -Path $_.FullName -Raw;
    $isDataTouched  = $false;
    
    Write-Host "Looking for Token: $($env:EnvironmentManagementUrlToken)"
    if($rawData -match "$($env:EnvironmentManagementUrlToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:EnvironmentManagementUrlToken)", "$($env:EnvironmentManagementUrl)";
        Write-Host "REPLACED Environment Management Url in file: $($_.FullName)";
    }

    Write-Host "Looking for Token: $($env:AppClientIdToken)"
    if($rawData -match "$($env:AppClientIdToken)") {        
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:AppClientIdToken)", "$($env:AppClientId)";
        Write-Host "REPLACED App Client Id in file: $($_.FullName)";
    }

    Write-Host "Looking for Token: $($env:B2CAuthUrlToken)"
    if($rawData -match "$($env:B2CAuthUrlToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:B2CAuthUrlToken)", "$($env:B2CAuthUrl)";
        Write-Host "REPLACED B2C Auth Url in file: $($_.FullName)";
    }

    Write-Host "Looking for Token: $($env:B2CTenantUrlToken)"
    if($rawData -match "$($env:B2CTenantUrlToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:B2CTenantUrlToken)", "$($env:B2CTenantUrl)" ;
        Write-Host "REPLACED B2C Tenant Url in file: $($_.FullName)";
    }

    Write-Host "Looking for Token: $($env:SignInPolicyToken)"
    if($rawData -match "$($env:SignInPolicyToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:SignInPolicyToken)", "$($env:SignInPolicy)";
        Write-Host "REPLACED Sign In Policy in file: $($_.FullName)";
    } 

    Write-Host "Looking for Token: $($env:AppInsightsInstrumentKeyToken)"
    if($rawData -match "$($env:AppInsightsInstrumentKeyToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace  "$($env:AppInsightsInstrumentKeyToken)", "$($env:AppInsightsInstrumentKey)";
        Write-Host "REPLACED App Insights Instrument Key in file: $($_.FullName)";
    }

    # Used for SpecFlow automation
    Write-Host "Looking for Token: $($env:BaseUrlWebsiteToken)"
    if($rawData -match "$($env:BaseUrlWebsiteToken)") {
        $isDataTouched = $true;
        $rawData = $rawData -replace "$($env:BaseUrlWebsiteToken)", "$($env:BaseUrlWebsite)";
        Write-Host "Replaced Base Url in file: $($_.FullName)";
    }

    if($isDataTouched) {
        Write-Host "Data was touched, replacing data in file"
         $rawData | Set-Content -Path $_;
    }
    Write-Host "END word match on file: $($_.FullName)"
}