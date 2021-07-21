$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$B2CAssets = "$($env:System_DefaultWorkingDirectory)/B2CAssets";
$Scripts = "$($env:System_DefaultWorkingDirectory)/Scripts";
$Environment = $env:Environment;
$StorageAccountName = "stratuswebsite$($Environment.ToLower())";

try {    
$ErrorActionPreference = 'Stop'
# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "x" "$($PSScriptRoot)/UserSignUpSignIn_*.zip" "-o$($B2CAssets)";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_DevOpsScripts/DevOps/PowerShell_Scripts_*.zip" "-o$($Scripts)";

Write-Host "...Replacing Configuration Variables on website/index.html...";
# Replace variables on index.html 
powershell.exe -file "$($Scripts)/Replace_Config_Variables.ps1" -PathToFile "$($B2CAssets)/website/index.html" 2>&1;

Write-Host "...Replacing Environment Tokens on azureResources/termsOfUse/consentPage.html...";
# Replace tokens on constentPage.html 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/azureResources/termsOfUse/consentPage.html" 2>&1;

Write-Host "...Replacing Environment Tokens on azureResources/Login/LoginCustom.html...";
# Replace tokens on LoginCustom.html 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/azureResources/Login/LoginCustom.html" 2>&1;

Write-Host "...Replacing Environment Tokens on azureResources/TrustFrameworkBase.xml...";
# Replace tokens on TrustFrameworkBase.xml 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens.ps1" -PathToFile "$($B2CAssets)/azureResources/TrustFrameworkBase.xml" 2>&1;


# Upload the TrustFramework .xml files from the GraphApi...

Write-Host "...Uploading Policy: B2C_1A_TrustFrameworkBase via GraphAPI...";
powershell.exe -file "$($Scripts)/Deploy_B2C_Assets.ps1" -PolicyId B2C_1A_TrustFrameworkBase -PathToFile "$($B2CAssets)/azureResources/TrustFrameworkBase.xml" -Environment $Environment 2>&1;

Write-Host "...Uploading Policy: B2C_1A_TrustFrameworkExtensions via GraphAPI...";
powershell.exe -file "$($Scripts)/Deploy_B2C_Assets.ps1" -PolicyId B2C_1A_TrustFrameworkExtensions -PathToFile "$($B2CAssets)/azureResources/TrustFrameworkExtensions.xml" -Environment $Environment 2>&1;

Write-Host "...Uploading Policy: B2C_1A_signup via GraphAPI...";
powershell.exe -file "$($Scripts)/Deploy_B2C_Assets.ps1" -PolicyId B2C_1A_signup -PathToFile "$($B2CAssets)/azureResources/SignUp.xml" -Environment $Environment 2>&1;

Write-Host "...Uploading Policy: B2C_1A_signup_signin via GraphAPI...";
powershell.exe -file "$($Scripts)/Deploy_B2C_Assets.ps1" -PolicyId B2C_1A_signup_signin -PathToFile "$($B2CAssets)/azureResources/SignUpOrSignin.xml" -Environment $Environment 2>&1;

Write-Host "Uploading Marketing Page to blob storage";
$MarketingUploadResults = az storage blob upload --container-name '$web' --account-name $StorageAccountName --file "$($B2CAssets)/website/index.html" --name index.html;
$MarketingUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'$web';

Write-Host "Uploading Terms of Consent blob storage";
$MarketingUploadResults = az storage blob upload-batch --container-name 'b2cassets' --account-name $StorageAccountName --file "$($B2CAssets)/azureResources/termsOfUse/*";
$MarketingUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'b2cassets';

Write-Host "Uploading Terms of Service to blob storage";
$MarketingUploadResults = az storage blob upload --container-name '$web' --name "TermsOfService" --account-name $StorageAccountName --file "$($B2CAssets)/termsOfService/index.html" --name index.html;
$MarketingUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'$web/TermsOfService';

Write-Host "Uploading LoginCustom to blob storage";
$MarketingUploadResults = az storage blob upload --container-name 'b2cassets' --account-name $StorageAccountName --file "$($B2CAssets)/azureResources/Login/LoginCustom.html" --name LoginCustom.html;
$MarketingUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'b2cassets';

} catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}