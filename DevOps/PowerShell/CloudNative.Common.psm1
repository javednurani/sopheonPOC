# PowerShell Module for common and recurring variables, .exe's, and functions 

$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

function Check-LastExitCode {
    if ($LASTEXITCODE -ne 0) {
        throw "Last Exit Code was not 0 but instead: $($LASTEXITCODE)";
    }
}


Export-ModuleMember -Function *
Export-ModuleMember -Variable *