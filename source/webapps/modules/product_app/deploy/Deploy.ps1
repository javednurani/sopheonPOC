$ZipUtil = "C:\Program Files\7-Zip\7z.exe";
$WebApp = "$($env:System_DefaultWorkingDirectory)/_App";

$Environment = $env:Environment;
$StorageAccountName = "stratuswebsite$($Environment.ToLower())";

try {    
    $ErrorActionPreference = 'Stop';
    # Zip/Archive Scripts 
    Write-Host "Zipping Artfacts for App...";
    & $ZipUtil "x" "$($PSScriptRoot)/App_*.zip" "-o$($WebApp)";

    & "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c "$($env:System_DefaultWorkingDirectory)\_StratusProductStoryApp\App\Product_Story_App_Configuration.json" -f "$WebApp\*"  -e $Environment

    Write-Output "Deleting existing web app files to reduce blob size";
    $DeleteStorage = az storage blob delete-batch --account-name $StorageAccountName --source '$web' --pattern 'product\*' --auth-mode login;
    $DeleteStorage;

    Write-Host "Uploading Marketing Page to blob storage";
    $ShellAppUploadResults = az storage blob upload-batch --destination '$web' --destination-path 'product' --account-name $StorageAccountName --source "$($WebApp)";
    $ShellAppUploadResults;
    Write-Host "Complete! Transfered files to Storage Account Blob: "'$web/product';
}
catch {
    Write-Host "ERROR: ";
    Write-Host $output;
    Write-Host $_;
    Write-Host $_.exception;
    exit 1;
}
