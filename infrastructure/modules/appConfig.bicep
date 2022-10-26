param location string
param appConfName string
@allowed([
  'free'
  'standard'
])
param configSku string = 'standard'

resource config 'Microsoft.AppConfiguration/configurationStores@2020-06-01' = {
  name: appConfName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  sku: {
    name: configSku
  }
}

output appConfigConnectionString string = listKeys(config.id, config.apiVersion).value[0].connectionString
