param(
    
    [string]$SubscriptionId = "c073dbb4-c517-4b8b-9614-3bfe50abeb2d", #Remove THIS!! should not be public and unique for each group member! 
    [string]$ResourceGroupName = "FitLifeDigital_RG",
    [string]$Location = "poland central",

    # Must be globally unique, lowercase, 5-50 characters, only letters/numbers.
    # Example: fitlifedigitaljfl
    
    [string]$AcrName = "fitlifedigital", #Change this to a unique name
    [string]$Sku = "Standard"

)

Write-Host "=== Fitlife Digital - Azure Container Registry setup ===" -ForegroundColor Cyan

# 1. Check Azure CLI
if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
    Write-Error "Azure CLI is not installed. Install Azure CLI first, then run this script again."
    exit 1
}

# 2. Login if needed
$account = az account show 2>$null | ConvertFrom-Json

if ($null -eq $account) {
    Write-Host "You are not logged in to Azure. Opening login..." -ForegroundColor Yellow
    az login
}

# 3. Select subscription if provided
if ($SubscriptionId -ne "") {
    Write-Host "Setting Azure subscription to: $SubscriptionId" -ForegroundColor Yellow
    az account set --subscription $SubscriptionId
}

$currentAccount = az account show | ConvertFrom-Json

Write-Host "Using subscription:" -ForegroundColor Green
Write-Host "  Name: $($currentAccount.name)"
Write-Host "  Id:   $($currentAccount.id)"

# 4. Validate ACR name locally before Azure call
if ($AcrName -notmatch '^[a-z0-9]{5,50}$') {
    Write-Error "Invalid ACR name '$AcrName'. Use 5-50 characters, lowercase letters and numbers only. No dashes, spaces, or underscores."
    exit 1
}

# 5. Create resource group
# Write-Host "Creating resource group '$ResourceGroupName' in '$Location'..." -ForegroundColor Cyan

# az group create `
#    --name $ResourceGroupName `
#    --location $Location `
#    --tags project=fitlife-digital

# if ($LASTEXITCODE -ne 0) {
#    Write-Error "Failed to create resource group."
#    exit 1
# }

# 6. Create Azure Container Registry
Write-Host "Creating Azure Container Registry '$AcrName'..." -ForegroundColor Cyan

az acr create `
    --resource-group $ResourceGroupName `
    --name $AcrName `
    --location $Location `
    --sku $Sku `
    --admin-enabled false `
    --tags project=fitlife-digital

if ($LASTEXITCODE -ne 0) {
    Write-Error "Failed to create Azure Container Registry. The name may already be taken globally."
    exit 1
}

# 7. Get registry login server
$loginServer = az acr show `
    --name $AcrName `
    --resource-group $ResourceGroupName `
    --query loginServer `
    --output tsv

Write-Host ""
Write-Host "Azure Container Registry created successfully!" -ForegroundColor Green
Write-Host "Resource group: $ResourceGroupName"
Write-Host "ACR name:       $AcrName"
Write-Host "Login server:   $loginServer"
Write-Host ""

# 8. Login Docker to ACR
Write-Host "Logging Docker into ACR..." -ForegroundColor Cyan

az acr login --name $AcrName

if ($LASTEXITCODE -ne 0) {
    Write-Warning "ACR was created, but Docker login failed. Make sure Docker Desktop is running, then run:"
    Write-Host "az acr login --name $AcrName"
}
else {
    Write-Host "Docker is now logged in to ACR." -ForegroundColor Green
}

Write-Host ""
Write-Host "Next image tag format:" -ForegroundColor Cyan
Write-Host "$loginServer/userservice:latest"
Write-Host "$loginServer/authservice:latest"
Write-Host "$loginServer/frontend:latest"
Write-Host "$loginServer/gateway:azure"
Write-Host ""
Write-Host "Example push:" -ForegroundColor Cyan
Write-Host "docker build -t $loginServer/userservice:latest -f UserService/UserService/Dockerfile ."
Write-Host "docker push $loginServer/userservice:latest"