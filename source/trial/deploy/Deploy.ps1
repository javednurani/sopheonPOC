$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$ShellApp = "$($env:System_DefaultWorkingDirectory)/ShellApp";
$Scripts = "$($env:System_DefaultWorkingDirectory)/Scripts";
$Environment = $env:Environment;
$StorageAccountName = "stratuswebsite$($Environment.ToLower())";

try {    
$ErrorActionPreference = 'Stop'
# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for ShellApp...";
& $ZipUtil "x" "$($PSScriptRoot)/ShellTrial_*.zip" "-o$($ShellApp)";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_DevOpsScripts/DevOps/PowerShell_Scripts_*.zip" "-o$($Scripts)";


Write-Host "...Replacing Environment Tokens on azureResources/termsOfUse/consentPage.html...";
# Replace tokens on constentPage.html 
powershell.exe -file "$($Scripts)/Replace_Environment_Tokens_ReactApp.ps1" 2>&1;

Write-Output "Deleting existing web app files to reduce blob size"
$DeleteStorage = az storage blob delete-batch --account-name $StorageAccountName --source '$web' --pattern 'WebApp/*' --auth-mode login;
$DeleteStorage;

Write-Host "Uploading Marketing Page to blob storage";
$ShellAppUploadResults = az storage blob upload-batch --destination '$web' --destination-path 'WebApp/' --account-name $StorageAccountName --source "$($ShellApp)/*";
$ShellAppUploadResults;
Write-Host "Complete! Transfered files to Storage Account Blob: "'$web/WebApp';

} catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}
