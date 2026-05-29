param(
    
    [string]$AcrName = "fitlifedigital",

    [string]$ResourceGroupName = "FitLifeDigital_RG",

    [string]$ImageTag = "latest"
)

Write-Host "=== Fitlife Digital - Build and Push Images ===" -ForegroundColor Cyan

# 1. Check Azure CLI
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Error "Azure CLI is not installed."
    exit 1
}

# 2. Check Docker
if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    Write-Error "Docker is not installed or not available in PATH."
    exit 1
}

# 3. Get ACR login server
Write-Host "Getting ACR login server..." -ForegroundColor Cyan

$loginServer = az acr show `
    --name $AcrName `
    --resource-group $ResourceGroupName `
    --query loginServer `
    --output tsv

if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($loginServer)) {
    Write-Error "Could not find ACR '$AcrName' in resource group '$ResourceGroupName'."
    exit 1
}

Write-Host "Using registry: $loginServer" -ForegroundColor Green

# 4. Login Docker to ACR
Write-Host "Logging into ACR..." -ForegroundColor Cyan
az acr login --name $AcrName

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to login to ACR. Make sure Docker Desktop is running."
    exit 1
}

# 5. Define images
$images = @(
    @{
        Name = "authservice"
        Dockerfile = "AuthService\AuthServiceAPI\Dockerfile"
        Context = "AuthService\AuthServiceAPI"
    },
    @{
        Name = "classservice"
        Dockerfile = "ClassServiceAPI\Dockerfile"
        Context = "ClassServiceAPI"
    },
    @{
        Name = "facilityservice"
        Dockerfile = "FacilityService\FacilityServiceAPI\Dockerfile"
        Context = "FacilityService\FacilityServiceAPI"
    },
    @{
        Name = "frontend"
        Dockerfile = "FitLife.Frontend\Dockerfile"
        Context = "FitLife.Frontend"
    },
    @{
        Name = "messageservice"
        Dockerfile = "MessageServiceAPI\Dockerfile"
        Context = "MessageServiceAPI"
    },
    @{
        Name = "ptservice"
        Dockerfile = "PTService\PTServiceAPI\Dockerfile"
        Context = "PTService\PTServiceAPI"
    },
    @{
        Name = "userservice"
        Dockerfile = "UserService\UserService\Dockerfile"
        Context = "UserService\UserService"
    },
    @{
        Name = "statisticservice"
        Dockerfile = "StatisticService\StatisticServiceAPI\Dockerfile"
        Context = "StatisticService\StatisticServiceAPI"
    },
    @{
        Name ="nginxgateway"
        Dockerfile = "nginxForAzure\Dockerfile"
        Context ="nginxForAzure"
    }
)

# 6. Build and push images
foreach ($image in $images) {
    $imageName = $image.Name
    $dockerfile = $image.Dockerfile
    $context = $image.Context
    $fullImageName = "$loginServer/$imageName`:$ImageTag"

    Write-Host ""
    Write-Host "----------------------------------------" -ForegroundColor DarkGray
    Write-Host "Building image: $fullImageName" -ForegroundColor Cyan
    Write-Host "Dockerfile: $dockerfile"
    Write-Host "Context:    $context"
    Write-Host "----------------------------------------" -ForegroundColor DarkGray

    if (-not (Test-Path $dockerfile)) {
        Write-Error "Dockerfile not found: $dockerfile"
        exit 1
    }

    if (-not (Test-Path $context)) {
        Write-Error "Build context folder not found: $context"
        exit 1
    }

    docker build `
        -t $fullImageName `
        -f $dockerfile `
        $context

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Docker build failed for $imageName."
        exit 1
    }

    Write-Host "Pushing image: $fullImageName" -ForegroundColor Cyan

    docker push $fullImageName

    if ($LASTEXITCODE -ne 0) {
        Write-Error "Docker push failed for $imageName."
        exit 1
    }

    Write-Host "Successfully pushed: $fullImageName" -ForegroundColor Green
}

Write-Host ""
Write-Host "All available Fitlife Digital images were built and pushed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Images pushed:" -ForegroundColor Cyan

foreach ($image in $images) {
    Write-Host "$loginServer/$($image.Name):$ImageTag"
}