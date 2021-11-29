[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.Security.SecureString]
    $Password = $(Throw "Need a password")
)

$ConvertedValue = ConvertFrom-SecureString -SecureString $Password -AsPlainText

Push-Location -Path "$($PSScriptRoot)\..\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Data"

Start-Process docker-compose -ArgumentList "-f docker-compose.dev-db.yml up -d --build"

Pop-Location