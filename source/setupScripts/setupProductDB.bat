@echo off

set password=%1

cd ..\Sopheon.CloudNative.Products\Sopheon.CloudNative.Products.DataAccess

dotnet ef database update -- --connectionstring "Server=.;Database=prodm;User Id=sa;Password=%password%; Trusted_Connection=False;MultipleActiveResultSets=true"
