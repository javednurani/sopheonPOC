$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$B2CAssets = "$($env:System_DefaultWorkingDirectory)/B2CAssets";
$Scripts = "$($env:System_DefaultWorkingDirectory)/Scripts";

try {
# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "x" "$($PSScriptRoot)/UserSignUpSignIn_*.zip" "-o$($B2CAssets)";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_DevOpsScripts/DevOps/PowerShell_Scripts_*.zip" "-o$($Scripts)";

$ErrorActionPreference = 'Stop'
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


} catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}