# Stop all containers.
docker ps -a -q | ForEach-Object { docker rm $_ }

# Delete all untagged local images.
docker images | ConvertFrom-String | Where-Object {$_.P2 -eq "<none>"} | ForEach-Object { docker rmi $_.P3 }

# Delete all volumes not used by containers.
docker volume prune -f