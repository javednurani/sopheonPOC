[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.Security.SecureString]
    $Password = $(Throw "Need a password")
)

Push-Location "$($PSScriptRoot)\source\Sopheon.CloudNative.Environments\docker";

docker-compose -f docker-compose.dev-db.yml up -d --build;

Write-Output "Waiting for docker db to startup...";
Start-Sleep -Seconds 5;

Push-Location ..\..\setupScripts

.\SetupEnvDB.ps1 -Password $Password

.\SetupProductDB.ps1 -Password $Password