Push-Location -Path "$($PSScriptRoot)\..\Sopheon.CloudNative.Environments\docker";

docker-compose -f docker-compose.dev-db.yml up -d --build;

Pop-Location;