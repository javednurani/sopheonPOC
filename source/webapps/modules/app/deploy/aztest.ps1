 $list = az storage blob list --account-name stratuswebsitedev2 --container-name '$web' --prefix 'WebApp/' --auth-mode login | ConvertFrom-Json

$deleteBatch = az storage blob delete-batch --account-name stratuswebsitedev2 --source '$web' --pattern  'WebApp/[!app1&&!images]*'

 #$list | ForEach-Object { Write-Host $_.name }

 $deleteBatch