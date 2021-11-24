$ZipUtil = "C:\Program Files\7-Zip\7z.exe";

#Adding blank comments to avoid bad artifact generation

$ProductManagementPath = "$($env:System_DefaultWorkingDirectory)\ProductManagementApi";
$ResourceGroup = "Stratus-$($env:Environment)";

# $azureKeyVault = "Cloud-DevOps";
# if ($env:AzureEnvironment -eq "Prod") {
#     $azureKeyVault = "Prod-Cloud-DevOps";
# }


$WebApiAppServiceName = "stratus-productmanagement-$env:Environment";

& $ZipUtil "x" "$($PSScriptRoot)/ProductManagement.zip" "-o$($ProductManagementPath)";

& "$($env:System_DefaultWorkingDirectory)\_TokenConfigurationManagement\TokenConfigManagement\TokenReplacer.exe" replace -c _ProductManagement\ProductManagement\Product_Management_Configuration.json -f "$ProductManagementPath\*"  -e $env:Environment

& $ZipUtil "a" "-tzip" "$env:Build_ArtifactStagingDirectory" $ProductManagementPath


#upload webapp app
az webapp deployment source config-zip --name $WebApiAppServiceName --resource-group $ResourceGroup --src "ProductManagementApi.zip";