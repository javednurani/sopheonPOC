$secretNames = az keyvault secret list --vault-name Stratus-Dev | ConvertFrom-Json

$secretNames.foreach{
    $value = (az keyvault secret show --vault-name Stratus-Dev --name $_.name | ConvertFrom-Json).value;
    az keyvault secret set --name $_.name --vault-name Stratus-Neptune --value $value    
}