param location string
param appConfName string
@allowed([
  'free'
  'standard'
])
param configSku string = 'standard'
param keyVaultName string
param webPrincipalId string

resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  // Make sure the Key Vault name begins with a letter.
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: config.identity.principalId
        permissions: {
          // Secrets are referenced by and enumerated in App Configuration so 'list' is not necessary.
          secrets: [
            'get'
          ]
        }
      }
      {
        tenantId: subscription().tenantId
        objectId: webPrincipalId
        permissions: {
          secrets: [
            'get'
          ]
        }
      }
    ]
  }
}

resource kvSecret 'Microsoft.KeyVault/vaults/secrets@2019-09-01' = {
  name: '${kv.name}/healthchecksecret'
  properties: {
    contentType: 'text/plain'
    value: 'true'
  }
}

resource config 'Microsoft.AppConfiguration/configurationStores@2020-06-01' = {
  name: appConfName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: configSku
  }
  
  resource configValue 'keyValues@2020-07-01-preview' = {
    // Store non-secrets in App Configuration e.g., client IDs, endpoints without secure tokens, etc.
    name: 'AppSettings:Healthcheck:AppConfig'
    properties: {
      contentType: 'text/plain'
      value: 'true'
    }
    
  }

  resource configSecret 'keyValues@2020-07-01-preview' = {
    // Store secrets in Key Vault with a reference to them in App Configuration e.g., client secrets, connection strings, etc.
    name: 'AppSettings:Healthcheck:KeyVault'
    properties: {
      // Most often you will want to reference a secret without the version so the current value is always retrieved.
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kvSecret.properties.secretUri}"}'
    }
  }
}

// Let the web app principal id access app config using Managed Identity
param roleDefinitionId string = '516239f1-63e1-4d78-a4de-a74fb236a071' // Default as App Configuration Data Reader role
resource webAppConfigRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  scope: config
  name: guid('ra-logicapp-${roleDefinitionId}')
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: webPrincipalId
  }
}

output appConfigEndpoint string = config.properties.endpoint
output vaultUrl string = kv.properties.vaultUri
output healthcheckSecretUri string = kvSecret.properties.secretUri 
