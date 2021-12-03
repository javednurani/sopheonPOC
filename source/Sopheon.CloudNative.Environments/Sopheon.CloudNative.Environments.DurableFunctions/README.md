

Creating a Service Principal for local authentication against Azure Resources
https://docs.microsoft.com/en-us/dotnet/azure/sdk/authentication

```
az ad sp create-for-rbac --sdk-auth
```

Save Variables to local.settings.json
AzSpTenantId
AzSpClientId
AzSpClientSecret

Other Settings or Secrets needed:
AzSqlServerName
SqlServerAdminEnigma
SQLCONNSTR_EnvironmentsSqlConnectionString
ARM_Template_BlobStorage_ConnectionString

