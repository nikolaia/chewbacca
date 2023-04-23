param sqlServerName string

@description('Specifies region for all resources')
param location string

@description('Specifies sql admin login')
param sqlAdministratorLogin string

@description('Specifies sql admin password')
@secure()
param sqlAdministratorPassword string

param adminIdentitySid string

@description('Specifies database name')
param sqlDatabaseName string

resource sqlserver 'Microsoft.Sql/servers@2020-11-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdministratorLogin
    administratorLoginPassword: sqlAdministratorPassword
    version: '12.0'
    administrators: {
      administratorType: 'ActiveDirectory'
      login: 'webappAppLogin'
      sid: adminIdentitySid
      tenantId: subscription().tenantId
      azureADOnlyAuthentication: true
    }
  }

  resource database 'databases@2020-08-01-preview' = {
    name: sqlDatabaseName
    location: location
    sku: {
      name: 'Basic'
    }
    properties: {
      collation: 'SQL_Latin1_General_CP1_CI_AS'
      maxSizeBytes: 1073741824
    }
  }

  resource firewallRule 'firewallRules@2020-11-01-preview' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      endIpAddress: '0.0.0.0'
      startIpAddress: '0.0.0.0'
    }
  }
}

output sqlConnectionString string = 'Server=tcp:${reference(sqlServerName).fullyQualifiedDomainName},1433; Initial Catalog=${sqlDatabaseName};Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False'
