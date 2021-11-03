# If running in a pipeline then use the Agent Home directory,
# otherwise use the machine temp folder which is useful for testing
if ($env:AGENT_HOMEDIRECTORY -ne $null) { $TargetFolder = $env:AGENT_HOMEDIRECTORY }
else { $TargetFolder = [System.Environment]::GetEnvironmentVariable('TEMP','Machine') }

# Loop through each CA in the machine store
Get-ChildItem -Path Cert:\LocalMachine\CA | ForEach-Object {

    # Convert cert's bytes to Base64-encoded text and add begin/end markers
    $Cert = "-----BEGIN CERTIFICATE-----`n"
    $Cert+= $([System.Convert]::ToBase64String($_.export([System.Security.Cryptography.X509Certificates.X509ContentType]::Cert),'InsertLineBreaks'))
    $Cert+= "`n-----END CERTIFICATE-----`n"

    # Append cert to chain
    $Chain+= $Cert
}

# Build target path
$CertFile = "$TargetFolder\TrustedRootCAs.pem"

# Write to file system
$Chain | Out-File $CertFile -Force -Encoding ASCII

# Clean-up
$Chain = $null

# Let Node (running later in the pipeline) know from where to read certs
Write-Host "##vso[task.setvariable variable=NODE.EXTRA.CA.CERTS]$CertFile"