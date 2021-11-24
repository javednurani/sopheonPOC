@echo off

set password=%1

cd ..\Sopheon.CloudNative.Environments\docker\

start docker-compose -f docker-compose.dev-db.yml up -d --build