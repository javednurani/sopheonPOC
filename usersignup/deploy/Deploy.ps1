$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$B2CAssets = "$($env:System_DefaultWorkingDirectory)\B2CAssets";
$Scripts = "$($env:System_DefaultWorkingDirectory)\Scripts";
$Environment = $env:Environment;
$StorageAccountName = "stratuswebsite$($Environment.ToLower())";

try {    
    $ErrorActionPreference = 'Stop'
    # Zip/Archive Scripts 
    Write-Host "Zipping Artfacts for UserSignUp...";
    & $ZipUtil "x" "$($PSScriptRoot)\UserSignUpSignIn_*.zip" "-o$($B2CAssets)";
    & $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)\_DevOpsScripts\DevOps\PowerShell_Scripts_*.zip" "-o$($Scripts)";

    Import-Module "$($Scripts)\CloudNative.Common.psm1";
    
    & "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _StratusB2CAssets\B2C\User_SignUp_Configuration.json -f "$B2CAssets\*"  -e $Environment

    # Upload the TrustFramework .xml files from the GraphApi...
    Write-Host "...Uploading B2C Custom Policies via GraphAPI...";
    powershell.exe -file "$($Scripts)\Deploy_B2C_Assets.ps1" -PathToFolder "$($B2CAssets)\azureResources\" -Environment $Environment;
    Check-LastExitCode;
   
    # Upload related B2C Assets to Blob Storage
    Write-Host "Uploading Marketing Page to blob storage";
    $MarketingUploadResults = az storage blob upload --container-name '$web' --account-name $StorageAccountName --file "$($B2CAssets)\website\index.html" --name index.html --auth-mode login;
    $MarketingUploadResults;
    Check-LastExitCode;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'$web';

    Write-Host "Uploading SelfAssertedTemplate blob storage";
    $MarketingUploadResults = az storage blob upload-batch --destination 'b2cassets' --account-name $StorageAccountName --source "$($B2CAssets)\azureResources\SelfAsserted" --auth-mode login;
    $MarketingUploadResults;
    Check-LastExitCode;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'b2cassets';

    Write-Host "Uploading Terms of Consent blob storage";
    $MarketingUploadResults = az storage blob upload-batch --destination 'b2cassets' --account-name $StorageAccountName --source "$($B2CAssets)\azureResources\termsOfUse" --auth-mode login;
    $MarketingUploadResults;
    Check-LastExitCode;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'b2cassets';

    Write-Host "Uploading Terms of Service to blob storage";
    $MarketingUploadResults = az storage blob upload --container-name '$web' --name "TermsOfService\index.html" --account-name $StorageAccountName --file "$($B2CAssets)\termsOfService\index.html" --auth-mode login;
    $MarketingUploadResults;
    Check-LastExitCode;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'$web\TermsOfService';

    Write-Host "Uploading LoginCustom to blob storage";
    $MarketingUploadResults = az storage blob upload --container-name 'b2cassets' --account-name $StorageAccountName --file "$($B2CAssets)\azureResources\Login\LoginCustom.html" --name LoginCustom.html --auth-mode login;
    $MarketingUploadResults;
    Check-LastExitCode;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'b2cassets';
}
catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}
