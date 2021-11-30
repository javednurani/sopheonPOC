@echo off

cd .\source\setupScripts\

start powershell -noexit .\startEnvService.bat

start powershell -noexit .\startProductService.bat

cd ..\webapps\

echo Build NPM and run.
start /wait powershell npm run buildpkgs
echo ..................
start /wait powershell npm run buildmods
echo .............
start /wait powershell npm install
echo ........
start powershell -noexit npm run start
echo ....
echo Done!
echo Services and react node server should be running in new windows...