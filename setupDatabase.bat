@echo off

set password=%1

cd .\source\Sopheon.CloudNative.Environments\docker\

docker-compose -f docker-compose.dev-db.yml up -d --build

echo
echo -n "Waiting for docker db to startup."
timeout /T 5

cd ..\..\setupScripts

.\setupEnvDB.bat %password%

.\setupProductDB.bat %password%