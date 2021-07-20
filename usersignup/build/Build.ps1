$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/usersignup/deploy/Deploy.ps1" -Destination "$($env:Build_ArtifactStagingDirectory)/Deploy.ps1";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/UserSignUpSignIn_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/usersignup/*" "-xr!usersignup/build" "-xr!usersignup/deploy";

Write-Host "Zipping Complete!";