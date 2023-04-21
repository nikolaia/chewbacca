param location string
param storageAccountName string
param containerName string = 'employees'

param webPrincipalId string

resource sa 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
  }
}

resource container 'Microsoft.Storage/storageAccounts/blobServices/containers@2022-05-01' = {
  name: '${sa.name}/default/${containerName}'
  properties: {
    publicAccess: 'Blob'
  }
}

// Storage Blob Data Contributor role
param roleDefinitionId string = 'ba92f5b4-2d11-453d-a403-e96b0029c9fe' 

// Let the web app principal id access storage account using Managed Identity
resource webAppConfigRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  scope: sa
  name: guid('chewie-sa-${roleDefinitionId}')
  properties: {
    principalType: 'ServicePrincipal'
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionId)
    principalId: webPrincipalId
  }
}

output employeeContainerEndpoint string = '${sa.properties.primaryEndpoints.blob}${containerName}'
output storageAccountName string = storageAccountName
output storageAccountId string = sa.id
