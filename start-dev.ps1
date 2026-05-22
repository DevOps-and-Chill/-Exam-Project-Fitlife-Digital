Write-Host ""
Write-Host "Ensuring backend exists..." -ForegroundColor Cyan

$networkExists = docker network ls --format "{{.Name}}" | Select-String "^backend$"

if (-not $networkExists) {
    docker network create backend | Out-Null
    Write-Host "Backend created" -ForegroundColor Green
}
else {
    Write-Host "Backend already exists" -ForegroundColor DarkGray
}

Write-Host ""
Write-Host "Starting compose-common..." -ForegroundColor Cyan

docker compose -f docker-compose-common.yml up -d

Write-Host ""
Write-Host "Waiting for Cosmos Emulator..." -ForegroundColor Yellow

$timeoutSeconds = 600
$start = Get-Date

do {
    Start-Sleep -Seconds 5

    $logs = docker logs cosmos 2>&1

    Write-Host "." -NoNewline

    if (((Get-Date) - $start).TotalSeconds -gt $timeoutSeconds) {
        Write-Host ""
        Write-Host ""
        Write-Host "Timeout waiting for Cosmos Emulator" -ForegroundColor Red
        exit 1
    }

}
until (
    $logs -match "(?m)^Started$"
)

Write-Host ""
Write-Host ""
Write-Host "Cosmos ready" -ForegroundColor Green

Write-Host ""
Write-Host "Starting compose-services (incl. gateway)..." -ForegroundColor Cyan

docker compose -f docker-compose-services.yml up -d

Write-Host ""
Write-Host "Done!" -ForegroundColor Green

Write-Host ""
Write-Host "Gateway:"
Write-Host "http://localhost:4000"

Write-Host ""
Write-Host "Eksempel:"
Write-Host "http://localhost:4000/user/user/addtestdata"