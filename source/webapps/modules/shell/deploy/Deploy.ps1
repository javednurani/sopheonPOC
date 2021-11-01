$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$ShellApp = "$($env:System_DefaultWorkingDirectory)/ShellApp";
$MarketingPage = "$($env:System_DefaultWorkingDirectory)/MarketingPage";

$Environment = $env:Environment;
$StorageAccountName = "stratuswebsite$($Environment.ToLower())";

try {    
    $ErrorActionPreference = 'Stop'
    # Zip/Archive Scripts 
    Write-Host "Zipping Artfacts for ShellApp...";
    & $ZipUtil "x" "$($PSScriptRoot)/Shell_*.zip" "-o$($ShellApp)";
    & $ZipUtil "x" "$($PSScriptRoot)/MarketingPage_*.zip" "-o$($MarketingPage)";

    & "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c "$($env:System_DefaultWorkingDirectory)\_StratusShellApp\ShellApp\Browser_Shell_Configuration.json" -f "$ShellApp\*"  -e $Environment
    & "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c "$($env:System_DefaultWorkingDirectory)\_StratusShellApp\ShellApp\Browser_Shell_Configuration.json" -f "$MarketingPage\*"  -e $Environment


    Write-Host "Uploading Marketing Page to blob storage";
    $MarketingUploadResults = az storage blob upload --container-name '$web' --account-name $StorageAccountName --file "$($MarketingPage)\index.html" --name 'Marketing/index.html' --auth-mode login;
    $MarketingUploadResults;    
    Write-Host "Complete! Transfered files to Storage Account Blob: "'$web';

    Write-Output "Deleting existing web app files to reduce blob size"
    $DeleteStorage = az storage blob delete-batch --account-name $StorageAccountName --source '$web' --pattern "[!app1!TermsOfService!Marketing]*" --auth-mode login;
    $DeleteStorage;

    Write-Host "Uploading Shell to blob storage";
    $ShellAppUploadResults = az storage blob upload-batch --destination '$web' --account-name $StorageAccountName --source "$($ShellApp)";
    $ShellAppUploadResults;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'$web';
}
catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}
