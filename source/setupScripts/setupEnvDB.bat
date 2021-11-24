@echo off

set password=%1

cd ..\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Data

dotnet ef database update -- --connectionstring "Server=.;Database=env;User Id=sa;Password=%password%; Trusted_Connection=False;MultipleActiveResultSets=true"