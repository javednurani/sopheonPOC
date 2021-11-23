Import-Module "$env:System_DefaultWorkingDirectory\DevOps\PowerShell\CloudNative.Common.psm1";
$ProductManagementProject = "$env:System_DefaultWorkingDirectory\source\Sopheon.CloudNative.Products\Sopheon.CloudNative.Products.AspNetCore\Sopheon.CloudNative.Products.AspNetCore.csproj";
$ProductsDataAccessProject = "$env:System_DefaultWorkingDirectory\source\Sopheon.CloudNative.Products\Sopheon.CloudNative.Products.DataAccess\Sopheon.CloudNative.Products.DataAccess.csproj";
$ProductsSourcePath = "$env:System_DefaultWorkingDirectory\source\Sopheon.CloudNative.Products";

#TODO: Does this need to be configurable....


Copy-Item -Path "$ProductsSourcePath\deploy\*" -Destination $env:Build_ArtifactStagingDirectory;

Set-Location -Path $ProductsSourcePath;

$OutputCoveragePath = "$ProductsSourcePath\TestResults\";

Write-Host "...Running dotnet ef migrations...";

dotnet ef migrations script -p $ProductsDataAccessProject -o "$env:Build_ArtifactStagingDirectory\products_migration.sql" -i -- --connectionstring "Server=.;Database=TenantBlankEnv;Integrated Security=true;"
Check-LastExitCode;

#Setup for Unit Tests here -
$TestProjects = Get-Item -Path "$ProductsSourcePath\**\*.UnitTests.csproj";
Write-Host "...Number of UnitTest projects found: $($TestProjects.Length)...";

Foreach($file in $TestProjects) {
    Write-Host "...Running unit tests on $($file.Name)...";
    dotnet test $file.FullName -p:CollectCoverage=true -p:CoverletOutput=$OutputCoveragePath -p:CoverletOutputFormat="json%2cCobertura" -p:MergeWith="$OutputCoveragePath\coverage.json" --logger:"xunit;LogFilePath=$($OutputCoveragePath)\$($file.Name.Replace('.csproj', '')).xml" -p:Exclude="[*]Sopheon.CloudNative.Products.DataAccess.Migrations.*"
}

#All migrations and tests are done...let's publish it!

Write-Host "...Running dotnet publish on Functions.csproj";
dotnet build $ProductManagementProject -c Release -o ".\PublishOutput\";
Check-LastExitCode;

dotnet tool restore
dotnet swagger tofile --output $env:Build_ArtifactStagingDirectory\swagger.json .\PublishOutput\Sopheon.CloudNative.Products.AspNetCore.dll v1
Check-LastExitCode;

# Zip/Archive Scripts 
Write-Host "Zipping Artfacts for Environment Management...";
& $ZipUtil "a" "-tzip" "$env:Build_ArtifactStagingDirectory\ProductManagement" ".\PublishOutput\*" "-xr!build" "-xr!deploy";
Check-LastExitCode;

Write-Host "Zipping Complete!";