[Cmdletbinding()]
Param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [System.Security.SecureString]
    $Password = $(Throw "Need a password")
)

$ConvertedValue = ConvertFrom-SecureString -SecureString $Password -AsPlainText;

Push-Location -Path "$($PSScriptRoot)\..\Sopheon.CloudNative.Products\Sopheon.CloudNative.Products.DataAccess";

dotnet ef database update -- --connectionstring "Server=.;Database=prodm;User Id=sa;Password=$($ConvertedValue); Trusted_Connection=False;MultipleActiveResultSets=true";

Pop-Location;