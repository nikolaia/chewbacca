param location string
param appConfName string
param keyVaultName string
param webPrincipalId string
param variantDevelopersRoleObjectId string

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
      {
        tenantId: subscription().tenantId
        objectId: variantDevelopersRoleObjectId
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
  parent: kv
  name: 'healthchecksecret'
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
    name: 'free'
  }
  
  resource configValueHealthcheck 'keyValues@2020-07-01-preview' = {
    name: 'AppSettings:Healthcheck:AppConfig'
    properties: {
      contentType: 'text/plain'
      value: 'true'
    }
  }

  resource configValueHealthcheckKeyVault 'keyValues@2020-07-01-preview' = {
    name: 'AppSettings:Healthcheck:KeyVault'
    properties: {
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kvSecret.properties.secretUri}"}'
    }
  }

  // Secret is populated from elsewhere
  resource configValueBemmaningConnectionString 'keyValues@2020-07-01-preview' = {
    name: 'AppSettings:BemanningConnectionString'
    properties: {
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kv.properties.vaultUri}secrets/BemanningConnectionString"}'
    }
  }

  // Secret is populated from elsewhere
  resource configValueCvPartnerToken 'keyValues@2020-07-01-preview' = {
    name: 'AppSettings:CvPartner:Token'
    properties: {
      contentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
      value: '{"uri":"${kv.properties.vaultUri}secrets/CvPartnerApiKey"}'
    }
  }
}

// App Configuration Data Reader role
param roleDefinitionId string = '516239f1-63e1-4d78-a4de-a74fb236a071' 

// Let the web app principal id access app config using Managed Identity
resource webAppConfigRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  scope: config
  name: guid('chewie-webapp-${roleDefinitionId}')
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: webPrincipalId
  }
}

resource variantDevelopersConfigRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  scope: config
  name: guid('chewie-developers-${roleDefinitionId}')
  properties: {
    principalType: 'Group'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: variantDevelopersRoleObjectId
  }
}

output appConfigEndpoint string = config.properties.endpoint
output vaultUrl string = kv.properties.vaultUri
output healthcheckSecretUri string = kvSecret.properties.secretUri 
