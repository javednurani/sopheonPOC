$ResourceGroup = "$($env:DeployResourceGroup)";

#region Tokens that we need to replace
#region App Service
$AppServiceNameToken = '&AppServiceName&';
$AppServiceNameValue = $ResourceGroup;
#endregion

#region Storage Account
$StorageAccountNameToken = '&StorageAccountName&';
$StorageAccountNameValue = $ResourceGroup.ToLower();
#endregion

#region SQL Server
$SqlServerNameToken = '&SqlServerName&';
$SqlServerNameValue = $ResourceGroup;
$SqlServerElasticPoolToken = '&SqlElasticPoolName&';
$SqlServerElasticPoolValue = "$($ResourceGroup)-Pool"
$SqlServerDatabaseNameToken = '&SqlServerDatabaseName&';
$SqlServerDatabaseNameValue = $ResourceGroup;
#endregion

#region CDN
$CDNProfileNameToken = '&CDNProfileName&';
$CDNProfileNameValue = $ResourceGroup;
$CDNProfileEndpointNameToken = '&CDNProfileEndpointName&';
$CDNProfileEndpointNameValue = $ResourceGroup;
$CDNProfileEndpointOriginToken = '&CDNProfileEndpointOrigin&';
$CDNProfileEndpointOriginValue = '';
#endregion

#region App Insights For SPA
$AppInsightsSpaNameValue = $ResourceGroup;
$AppInsightsSpaNameToken = '&AppInsightsSpaName&';
#endregion

#region Resource Group
$ResourceGroupToken = '&ResourceGroup&';
$ResourceGroupValue = $ResourceGroup;
#endregion

#region CodeBase Variables 
$EnvironmentManagementUrl = '';
#endregion

#endregion

Export-ModuleMember -Variable *;