@description('Name for the Azure Container Instance container group')
param containerGroupName string = 'fitlife-services'

@description('Azure region')
param location string = resourceGroup().location

@description('ACR image prefix')
param imageLocationPrefix string = 'fitlifedigitaljfl.azurecr.io'

@description('Image tag for custom FitLife images')
param imageTag string = 'latest'

@description('ACR username')
param acrUsername string

@secure()
@description('ACR password')
param acrPassword string

@description('Restart behavior for the container group')
@allowed([
  'Always'
  'Never'
  'OnFailure'
])
param restartPolicy string = 'Always'

param frontendPort int = 8080
param gatewayPort int = 4000

param cosmosAccountName string = 'fitlifedigitaljflcosmos'
param cosmosDatabaseName string = 'fitlife-digital-db'
param enableCosmosFreeTier bool = true

var authServicePort = 8081
var classServicePort = 8082
var facilityServicePort = 8083
var messageServicePort = 8084
var ptServicePort = 8085
var rapportServicePort = 8086
var userServicePort = 8087

var rabbitMqPort = 5672
var rabbitMqManagementPort = 15672
var lokiPort = 3100
var grafanaPort = 3000

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2024-11-15' = {
  name: cosmosAccountName
  location: location
  kind: 'GlobalDocumentDB'
  properties: {
    databaseAccountOfferType: 'Standard'
    enableFreeTier: enableCosmosFreeTier
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
  }
}

resource cosmosDatabase 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2024-11-15' = {
  parent: cosmosAccount
  name: cosmosDatabaseName
  properties: {
    resource: {
      id: cosmosDatabaseName
    }
  }
}

resource placeholderContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2024-11-15' = {
  parent: cosmosDatabase
  name: 'placeholder-container'
  properties: {
    resource: {
      id: 'placeholder-container'
      partitionKey: {
        paths: [
          '/id'
        ]
        kind: 'Hash'
      }
    }
  }
}

var cosmosConnectionString = 'AccountEndpoint=https://${cosmosAccount.name}.documents.azure.com:443/;AccountKey=${listKeys(cosmosAccount.id, cosmosAccount.apiVersion).primaryMasterKey};'

resource containerGroup 'Microsoft.ContainerInstance/containerGroups@2023-05-01' = {
  name: containerGroupName
  location: location
  properties: {
    osType: 'Linux'
    restartPolicy: restartPolicy

    imageRegistryCredentials: [
      {
        server: imageLocationPrefix
        username: acrUsername
        password: acrPassword
      }
    ]

    containers: [
      {
        name: 'frontend'
        properties: {
          image: '${imageLocationPrefix}/frontend:${imageTag}'
          ports: [
            {
              port: frontendPort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${frontendPort}'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'gateway'
        properties: {
          image: '${imageLocationPrefix}/gateway:azure'
          ports: [
            {
              port: gatewayPort
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.25')
            }
          }
        }
      }

      {
        name: 'authservice'
        properties: {
          image: '${imageLocationPrefix}/authservice:${imageTag}'
          ports: [
            {
              port: authServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${authServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'classservice'
        properties: {
          image: '${imageLocationPrefix}/classservice:${imageTag}'
          ports: [
            {
              port: classServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${classServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'facilityservice'
        properties: {
          image: '${imageLocationPrefix}/facilityservice:${imageTag}'
          ports: [
            {
              port: facilityServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${facilityServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'messageservice'
        properties: {
          image: '${imageLocationPrefix}/messageservice:${imageTag}'
          ports: [
            {
              port: messageServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${messageServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'ptservice'
        properties: {
          image: '${imageLocationPrefix}/ptservice:${imageTag}'
          ports: [
            {
              port: ptServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${ptServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'rapportservice'
        properties: {
          image: '${imageLocationPrefix}/rapportservice:${imageTag}'
          ports: [
            {
              port: rapportServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${rapportServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'userservice'
        properties: {
          image: '${imageLocationPrefix}/userservice:${imageTag}'
          ports: [
            {
              port: userServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${userServicePort}'
            }
            {
              name: 'RabbitMQ__Host'
              value: 'localhost'
            }
            {
              name: 'RabbitMQ__Port'
              value: '${rabbitMqPort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'CosmosDb__ConnectionString'
              secureValue: cosmosConnectionString
            }
            {
              name: 'CosmosDb__DatabaseName'
              value: cosmosDatabaseName
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'rabbitmq'
        properties: {
          image: 'rabbitmq:3-management'
          ports: [
            {
              port: rabbitMqPort
              protocol: 'TCP'
            }
            {
              port: rabbitMqManagementPort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'RABBITMQ_DEFAULT_USER'
              value: 'guest'
            }
            {
              name: 'RABBITMQ_DEFAULT_PASS'
              value: 'guest'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.5')
              memoryInGB: json('1.0')
            }
          }
        }
      }

      {
        name: 'loki'
        properties: {
          image: 'grafana/loki:latest'
          ports: [
            {
              port: lokiPort
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'grafana'
        properties: {
          image: 'grafana/grafana:latest'
          ports: [
            {
              port: grafanaPort
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.25')
              memoryInGB: json('0.5')
            }
          }
        }
      }
    ]

    ipAddress: {
      type: 'Public'
      ports: [
        {
          port: frontendPort
          protocol: 'TCP'
        }
        {
          port: gatewayPort
          protocol: 'TCP'
        }
      ]
    }
  }

  dependsOn: [
    placeholderContainer
  ]
}

output containerGroupName string = containerGroup.name
output containerIPv4Address string = containerGroup.properties.ipAddress.ip
output frontendUrl string = 'http://${containerGroup.properties.ipAddress.ip}:${frontendPort}'
output gatewayUrl string = 'http://${containerGroup.properties.ipAddress.ip}:${gatewayPort}'
output cosmosAccount string = cosmosAccount.name
output cosmosDatabase string = cosmosDatabase.name
