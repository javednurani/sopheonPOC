Push-Location -Path "$($PSScriptRoot)\..\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Data";

docker-compose -f docker-compose.dev-db.yml up -d --build;

Pop-Location;