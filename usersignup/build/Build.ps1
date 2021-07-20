$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "a" "-tzip" "$($env:Build.ArtifactStagingDirectory)/UserSignUpSignIn_$($env:Build.BuildId)" "$($env:System.DefaultWorkingDirectory)/usersignup";

Write-Host "Zipping Complete!";