$ZipUtil = "C:\Program Files\7-Zip\7z.exe";



# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "x" "$($env:System_DefaultWorkingDirectory)/_StratusB2CAssets/UserSignUpSignIn_*.zip" "-o$($env:System_DefaultWorkingDirectory)/B2CAssets";