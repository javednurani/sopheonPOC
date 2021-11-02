#Adding blank comments to avoid bad artifact generation

$ResourceGroup = "Stratus-$($env:Environment)";

# $azureKeyVault = "Cloud-DevOps";
# if ($env:AzureEnvironment -eq "Prod") {
#     $azureKeyVault = "Prod-Cloud-DevOps";
# }

$WebApiAppServiceName = "stratus-productmanagement-$env:Environment"


#upload webapp app
az webapp deployment source config-zip --name $WebApiAppServiceName --resource-group $ResourceGroup --src "_ProductManagement\ProductManagement\ProductManagement.zip";
