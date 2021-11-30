# Running Locally

See: https://pluto/display/PDP/Running+Environment+Subsystem+and+Product+Management+API+locally

# Getting Started

This solution implements "Product Management" capabilities within a Tenant Environmetn Scope

Start Up Project: Sopheon.CloudNative.Products.Asp.NetCore

Initial Dev Environment Setup Steps: 
- Create some local tenant environment databases to connect against
- In Visual Studio command prompt, cd to the Sopheon.CloudNative.Products.DataAccess directory
- Run the dotnet ef command to create/update some example target databases

```
dotnet ef database update -- --connectionstring ".;Database=TenantEnvironment1;Integrated Security=True"
```

Update the appsettings.Development.json file within Sopheon.CloudNative.Products.AspNetCore to point to the example databases you created
- Update the LocalDevelopment.TenantEnvironmentDatabases section to define the environment databases to connect against
- Set LocalDevelopment.UseEnvironmentDatabasesFromAppSettings to true to work off this list in the config file, rather than connecting against a environment management webservice

The key of the TenantEnvironmentDatabase values will be used as the environment key for the product management api
Ex: 
```
/Environments/<environment key guid>/Attributes
```

# Solution Items

- .editorconfig file holds Sopheon VS Settings for Solution.  No dev action should be necessary.

# CI Notes
- Due to the use of a IDesignTimeDbContextFactory, a --connectionstring argument is required for 'dotnet ef' commands against a ProductManagementContext
- For commands NOT affecting a specific (eg, scripting migrations), connection string value does not need to be value, but cannot be an empty string.
- See stratus\source\Sopheon.CloudNative.Products\build\Build.ps1
- See stratus\source\Sopheon.CloudNative.Environments\build\Build.ps1