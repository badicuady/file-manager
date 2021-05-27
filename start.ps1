cls

docker build -t file.manager.api:local .

$container = docker ps -a | select-string filemanager-dev | out-string
$isUp = $container | select-string Up | out-string

if ($isUp.Length -gt 0) {
    Write-Host "Stopping filemanager-dev container on port 5000"
    & docker stop filemanager-dev
}

if ($container.Length -gt 0) {
    Write-Host "Removing filemanager-dev container on port 5000"
    & docker rm filemanager-dev
}

docker run --name filemanager-dev -p 5000:80 -d file.manager.api:local