param location string
param name string
@allowed([
  'free'
  'standard'
])
param configSku string = 'standard'

resource config 'Microsoft.AppConfiguration/configurationStores@2020-06-01' = {
  name: name
  location: location
  sku: {
    name: configSku
  }
}

output appConfigConnectionString string = listKeys(config.id, config.apiVersion).value[0].connectionString
