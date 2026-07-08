#!/usr/bin/env bash
set -e

# Update these values before running.
RESOURCE_GROUP="rg-enterprise-workbench"
LOCATION="centralindia"
ACR_NAME="eworkbenchacr$RANDOM"
KV_NAME="eworkbenchkv$RANDOM"
POSTGRES_SERVER="eworkbenchpg$RANDOM"
POSTGRES_DB="workbenchdb"
POSTGRES_ADMIN="workbenchadmin"
POSTGRES_PASSWORD="ChangeThisPassword123!"
ENV_NAME="eworkbench-env"

az group create --name "$RESOURCE_GROUP" --location "$LOCATION"

az acr create --resource-group "$RESOURCE_GROUP" --name "$ACR_NAME" --sku Basic --admin-enabled true
ACR_LOGIN_SERVER=$(az acr show --resource-group "$RESOURCE_GROUP" --name "$ACR_NAME" --query loginServer -o tsv)

az postgres flexible-server create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$POSTGRES_SERVER" \
  --location "$LOCATION" \
  --admin-user "$POSTGRES_ADMIN" \
  --admin-password "$POSTGRES_PASSWORD" \
  --sku-name Standard_B1ms \
  --tier Burstable \
  --version 16 \
  --storage-size 32 \
  --public-access 0.0.0.0

az postgres flexible-server db create \
  --resource-group "$RESOURCE_GROUP" \
  --server-name "$POSTGRES_SERVER" \
  --database-name "$POSTGRES_DB"

az keyvault create --resource-group "$RESOURCE_GROUP" --name "$KV_NAME" --location "$LOCATION"

POSTGRES_CONNECTION="Host=$POSTGRES_SERVER.postgres.database.azure.com;Port=5432;Database=$POSTGRES_DB;Username=$POSTGRES_ADMIN;Password=$POSTGRES_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
az keyvault secret set --vault-name "$KV_NAME" --name "ConnectionStrings--Postgres" --value "$POSTGRES_CONNECTION"

az containerapp env create --name "$ENV_NAME" --resource-group "$RESOURCE_GROUP" --location "$LOCATION"
az acr login --name "$ACR_NAME"

for service in IdentityService UserService DashboardService NotificationService; do
  image_name=$(echo "$service" | tr '[:upper:]' '[:lower:]')
  docker build -t "$ACR_LOGIN_SERVER/$image_name:latest" "../../services/$service"
  docker push "$ACR_LOGIN_SERVER/$image_name:latest"
done

az docker build --registry "$ACR_NAME" --image "client:latest" ../../client

create_app () {
  local app_name=$1
  local image_name=$2
  az containerapp create \
    --name "$app_name" \
    --resource-group "$RESOURCE_GROUP" \
    --environment "$ENV_NAME" \
    --image "$ACR_LOGIN_SERVER/$image_name:latest" \
    --target-port 8080 \
    --ingress external \
    --system-assigned \
    --env-vars "KeyVault__VaultUri=https://$KV_NAME.vault.azure.net/" "ASPNETCORE_URLS=http://+:8080"

  PRINCIPAL_ID=$(az containerapp identity show --name "$app_name" --resource-group "$RESOURCE_GROUP" --query principalId -o tsv)
  az role assignment create --assignee "$PRINCIPAL_ID" --role "Key Vault Secrets User" --scope "$(az keyvault show --name "$KV_NAME" --query id -o tsv)"
}

create_app "identity-service" "identityservice"
create_app "user-service" "userservice"
create_app "dashboard-service" "dashboardservice"
create_app "notification-service" "notificationservice"

echo "Azure deployment completed. Configure client API URLs from Container App FQDN values."
