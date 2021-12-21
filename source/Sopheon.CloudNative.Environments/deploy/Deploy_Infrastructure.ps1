[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $false)][string]$Environment = $env:Environment
)

function Test-LastExitCode() {
    if ($LASTEXITCODE -ne 0) {
        throw "Last Exit Code was not 0 but instead: $($LASTEXITCODE)";
    }
}

$azureKeyVault = "Cloud-DevOps";
if ($env:AzureEnvironment -eq "Prod") {
    $azureKeyVault = "Prod-Cloud-DevOps";
}

$SqlAdminEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "SqlServerAdminEnigma" --query value).Replace('"', '');
$AzSpClientEnigma = (az keyvault secret show --vault-name $azureKeyVault --name "AzSpClientEnigma" --query value).Replace('"', '');
$AzSpClientId = (az keyvault secret show --vault-name $azureKeyVault --name "AzSpClientId" --query value).Replace('"', '');

$azurePassword = ConvertTo-SecureString "$($AzSpClientEnigma)" -AsPlainText -Force
$psCred = New-Object System.Management.Automation.PSCredential($AzSpClientId , $azurePassword)
Connect-AzAccount -Credential $psCred -TenantId $env:Tenant -ServicePrincipal

$DeploymentName = "ADO-Deployment";

$ResourceGroupValue = "Stratus-$($Environment)";

#Token values
$EnvironmentFunctionAppName = "$($ResourceGroupValue.ToLower())-environment";
$FunctionAppStorageAccountName = "stratus$($Environment.ToLower())funcapp"
$AppInsightsName = $ResourceGroupValue;
$EnvironmentSQLServerName = $ResourceGroupValue;
$ElasticJobAgentSQLServerName = "$($ResourceGroupValue)-JobAgent";
$TenantSQLServerName = "$($ResourceGroupValue)-TenantEnvironments";
$EnvironmentManagementSQLServerDatabaseName = "EnvironmentManagement";
$ElasticJobAgentSQLServerDatabaseName = "JobAgent";
$ElasticJobAgentName = "JobAgent";
$WebServerFarmName = "ASP-$($ResourceGroupValue)-Environment";

$ResourceFunctionAppName = "$($ResourceGroupValue.ToLower())-resource";
$ResourceWebServerFarmName = "ASP-$($ResourceGroupValue)-Resource";

$MasterTemplate = "$($PSScriptRoot)\Master_Template.bicep";

Write-Host "Replacing tokens on Master Template...";
$masterTemplateContent = Get-Content $MasterTemplate -raw;
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentManagementSQLServerName^', $EnvironmentSQLServerName).Replace('^EnvironmentManagementSQLServerDatabaseName^', $EnvironmentManagementSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^ElasticJobAgentSQLServerName^', $ElasticJobAgentSQLServerName).Replace('^ElasticJobAgentSQLServerDatabaseName^', $ElasticJobAgentSQLServerDatabaseName);
$masterTemplateContent = $masterTemplateContent.Replace('^ElasticJobAgentName^', $ElasticJobAgentName)
$masterTemplateContent = $masterTemplateContent.Replace('^EnvironmentFunctionAppName^', $EnvironmentFunctionAppName).Replace('^TenantSQLServerName^', $TenantSQLServerName);
$masterTemplateContent = $masterTemplateContent.Replace('^AppInsightsName^', $AppInsightsName).Replace('^FunctionStorageAccountName^', $FunctionAppStorageAccountName);
$masterTemplateContent = $masterTemplateContent.Replace('^SqlAdminEngima^', $SqlAdminEnigma).Replace('^EnvironmentWebServerFarmName^', $WebServerFarmName);
$masterTemplateContent = $masterTemplateContent.Replace('^ResourceWebServerFarmName^', $ResourceWebServerFarmName).Replace('^ResourceManagementFunctionAppName^', $ResourceFunctionAppName);
Set-Content -Value $masterTemplateContent -Path $MasterTemplate;
Write-Host "Complete!";

& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _StratusEnvironmentManagement\EnvironmentManagement\Environments_Configuration.json -f "$PSScriptRoot\*"  -e $Environment
Test-LastExitCode;

Write-Host "Deploying Storage Account Template to Resource Group: $($ResourceGroupValue)";
# Creates a deployment for the given resource group and template.json
$GroupExists = az group exists --name $ResourceGroupValue;

if('false' -eq $GroupExists)
{
    Write-Host "Resource Group does not exist; Creating group: $($ResourceGroupValue)";
    az group create --location 'westus' --name $ResourceGroupValue --tags 'Environment Type=Development' 'Owner=Cal' 'Review Date=12-30-21' --query "properties.provisioningState";
}

Write-Host "Deploying Master Template...";
$MasterTemplateDeploy = az deployment group create --resource-group $ResourceGroupValue --template-file $MasterTemplate --name "$($DeploymentName)-MasterDeploy-EnvironmentManagement" --query "properties.provisioningState";
Test-LastExitCode;
Write-Host "Master Template Deployment: $($MasterTemplateDeploy)";

Write-Host "Configuring Elastic Job Agent Host..."
try {
    # Will throw error if the credential does not exist, when then know we need to create one
    Get-AzSqlElasticJobCredential -ResourceGroupName $ResourceGroupValue -ServerName $ElasticJobAgentSQLServerName -AgentName 'JobAgent' -Name 'masteruser';   
}
catch {
    Write-Host "Refresh credentials do not exist, creating..."
    $SecureSqlAdminEnigma1 = (ConvertTo-SecureString -String $SqlAdminEnigma -AsPlainText -Force)
    $masterCred = New-Object -TypeName "System.Management.Automation.PSCredential" -ArgumentList "masteruser", $SecureSqlAdminEnigma1
    New-AzSqlElasticJobCredential -ResourceGroupName $ResourceGroupValue -ServerName $ElasticJobAgentSQLServerName -Credential $masterCred -AgentName 'JobAgent' -Name 'masteruser'
}

try {
    # Will throw error if the credential does not exist, when then know we need to create one
    Get-AzSqlElasticJobCredential -ResourceGroupName $ResourceGroupValue -ServerName $ElasticJobAgentSQLServerName -AgentName 'JobAgent' -Name 'jobuser';
}
catch { 
    Write-Host "Job credentials do not exist, creating..."
    $SecureSqlAdminEnigma2 = (ConvertTo-SecureString -String $SqlAdminEnigma -AsPlainText -Force)
    $jobCred = New-Object -TypeName "System.Management.Automation.PSCredential" -ArgumentList "jobuser", $SecureSqlAdminEnigma2
    New-AzSqlElasticJobCredential -ResourceGroupName $ResourceGroupValue -ServerName $ElasticJobAgentSQLServerName -Credential $jobCred -AgentName 'JobAgent'  -Name 'jobuser'
}

Write-Host "Elastic Job Agent Host configured!"

$environmentManagementConnectionString = (az sql db show-connection-string --client ado.net --server "$($EnvironmentSQLServerName.ToLower())" --name $EnvironmentManagementSQLServerDatabaseName).Replace('"', '');

$environmentManagementConnectionString = $environmentManagementConnectionString.Replace('<username>', 'sopheon').Replace('<password>', $SqlAdminEnigma);

$connectionString1 = az webapp config connection-string set --resource-group $ResourceGroupValue --name $EnvironmentFunctionAppName -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;
$connectionString2 = az webapp config connection-string set --resource-group $ResourceGroupValue --name $ResourceFunctionAppName -t SQLServer --settings EnvironmentsSqlConnectionString=$environmentManagementConnectionString;
