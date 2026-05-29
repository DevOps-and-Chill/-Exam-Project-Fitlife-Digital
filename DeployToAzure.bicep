@description('Name for the Azure Container Instance container group')
param containerGroupName string = 'fitlife-services'

@description('Azure region')
param location string = 'poland central'

@description('ACR image prefix')
param imageLocationPrefix string = 'fitlifedigital.azurecr.io'

@description('Image tag for custom FitLife images')
param imageTag string = 'latest'

@description('ACR username')
param acrUsername string = 'fitlifedigital'

@secure()
@description('ACR password')
param acrPassword string = 'BQUCQ4NyKhYvGqEPjqm4Qc9dMDbnOZviu1By7Q1r5yHvu1t3tfIxJQQJ99CEACE1PydEqg7NAAACAZCRYNPa'

@description('Restart behavior for the container group')
@allowed([
  'Always'
  'Never'
  'OnFailure'
])
param restartPolicy string = 'Always'

@secure()
param vaultToken string = 'fitlife-root-token'

param frontendPort int = 8089
param gatewayPort int = 4000

var authServicePort = 8081
var classServicePort = 8082
var facilityServicePort = 8083
var messageServicePort = 8084
var ptServicePort = 8085
var statisticServicePort = 8086
var userServicePort = 8087
var digitalcontentservicePort = 8088

var rabbitMqPort = 5672
var rabbitMqManagementPort = 15672
var lokiPort = 3100
var grafanaPort = 3000
var vaultPort = 8200

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
              cpu: json('0.2')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'gateway'
        properties: {
          image: '${imageLocationPrefix}/gateway:${imageTag}'
          ports: [
            {
              port: gatewayPort
              protocol: 'TCP'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.3')
              memoryInGB: json('0.3')
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
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'statisticservice'
        properties: {
          image: '${imageLocationPrefix}/statisticservice:${imageTag}'
          ports: [
            {
              port: statisticServicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${rapportServicePort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
              memoryInGB: json('0.5')
            }
          }
        }
      }

      {
        name: 'digitalcontentservice'
        properties: {
          image: '${imageLocationPrefix}/digitalcontentservice:${imageTag}'
          ports: [
            {
              port: digitalcontentservicePort
              protocol: 'TCP'
            }
          ]
          environmentVariables: [
            {
              name: 'ASPNETCORE_URLS'
              value: 'http://+:${digitalcontentservicePort}'
            }
            {
              name: 'Loki__Url'
              value: 'http://localhost:${lokiPort}'
            }
            {
              name: 'Vault__Address'
              value: 'http://localhost:8200'
            }
            {
              name: 'VAULT_TOKEN'
              secureValue: vaultToken
            }
            {
              name: 'Vault__SecretPath'
              value: 'secret/fitlife'
            }
          ]
          resources: {
            requests: {
              cpu: json('0.2')
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
              cpu: json('0.2')
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
              cpu: json('0.2')
              memoryInGB: json('0.5')
            }
          }
        }
      }
      {
        name: 'vault'
        properties: {
          image: 'hashicorp/vault:1.20'

          environmentVariables: [
            {
              name: 'VAULT_DEV_ROOT_TOKEN_ID'
              value: vaultToken
            }
            {
              name: 'VAULT_DEV_LISTEN_ADDRESS'
              value: '0.0.0.0:8200'
            }
            {
              name: 'VAULT_DISABLE_MLOCK'
              value: 'true'
            }
            {
              name: 'GatewayBaseUrl' 
              value: http://localhost:4000/
            }
          ]

          ports: [
            {
              port: 8200
            }
          ]

          resources: {
            requests: {
              cpu: json('0.5')
              memoryInGB: json('1')
            }
          }
        }
      }
    ]
    ipAddress: {
      type: 'Public'

      dnsNameLabel: 'fitlife-gateway'

      ports: [
        {
          port: gatewayPort
          protocol: 'TCP'
        }
        {
          port: vaultPort
          protocol: 'TCP'
        }
      ]
    }
  }
  dependsOn: [
  ]
}

output containerGroupName string = containerGroup.name
output containerIPv4Address string = containerGroup.properties.ipAddress.ip
output frontendUrl string = 'http://${containerGroup.properties.ipAddress.ip}:${frontendPort}'
output gatewayUrl string = 'http://${containerGroup.properties.ipAddress.ip}:${gatewayPort}'

