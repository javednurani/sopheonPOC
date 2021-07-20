$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "a" "-tzip" "$($env:Build_ArtifactStagingDirectory)/UserSignUpSignIn_$($env:Build_BuildId)" "$($env:System_DefaultWorkingDirectory)/usersignup";

Write-Host "Zipping Complete!";