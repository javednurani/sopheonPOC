[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)][string]$PathToFile
)

#Environment should always be set from the Pipeline variables. If this isn't set, nothing is getting out
$Environment = $env:Environment;
$StorageAccount = $env:StorageAccountName

$fileContent = Get-Content $PathToFile;

$fileContent = $fileContent.Replace("^StorageAccountName^", $StorageAccount);

Set-Content -Path $PathToFile -Value $fileContent -Force;