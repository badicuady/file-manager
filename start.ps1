cls

docker build -t file.manager.api:local .
docker build -t file.manager.ui.api:local -f Dockerfile.ui .

$container1 = docker ps -a | select-string filemanager-api-dev | out-string
$isUp1 = $container1 | select-string Up | out-string

if ($isUp1.Length -gt 0) {
    Write-Host "Stopping filemanager-api-dev container on port 5000"
    & docker stop filemanager-api-dev
}

if ($container1.Length -gt 0) {
    Write-Host "Removing filemanager-api-dev container on port 5000"
    & docker rm filemanager-api-dev
}

$container2 = docker ps -a | select-string filemanager-ui-dev | out-string
$isUp2 = $container2 | select-string Up | out-string

if ($isUp2.Length -gt 0) {
    Write-Host "Stopping filemanager-ui-dev container on port 3000"
    & docker stop filemanager-ui-dev
}

if ($container2.Length -gt 0) {
    Write-Host "Removing filemanager-ui-dev container on port 3000"
    & docker rm filemanager-ui-dev
}

docker run --name filemanager-api-dev -p 5000:80 -d file.manager.api:local

docker run --name filemanager-ui-dev -p 3000:80 -d file.manager.ui.api:local