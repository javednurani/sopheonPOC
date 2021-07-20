$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for UserSignUp...";
& $ZipUtil "a" "-tzip" "$(Build.ArtifactStagingDirectory)/UserSignUpSignIn_$(Build.BuildId)" "$(System.DefaultWorkingDirectory)/usersignup";

Write-Host "Zipping Complete!";