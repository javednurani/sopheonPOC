Import-Module "$($env:System_DefaultWorkingDirectory)\DevOps\PowerShell\CloudNative.Common.psm1";

Copy-Item -Path "$($env:System_DefaultWorkingDirectory)/usersignup/deploy/*" -Destination "$($env:Build_ArtifactStagingDirectory)";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/UserSignUpSignIn_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/usersignup/*" "-xr!build" "-xr!deploy";

Write-Host "Zipping Complete!";