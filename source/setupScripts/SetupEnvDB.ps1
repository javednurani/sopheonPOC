[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.Security.SecureString]
    $Password = $(Throw "Need a password")
)

$ConvertedValue = ConvertFrom-SecureString -SecureString $Password -AsPlainText

Push-Location -Path "$($PSScriptRoot)\..\Sopheon.CloudNative.Environments\Sopheon.CloudNative.Environments.Data"

dotnet ef database update -- --connectionstring "Server=.;Database=env;User Id=sa;Password=$($ConvertedValue); Trusted_Connection=False;MultipleActiveResultSets=true"

Pop-Location