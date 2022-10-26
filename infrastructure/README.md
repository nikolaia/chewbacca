# WIP

```az
az group create -l westeurope -n <group-name> --tags Project=Chewbacca 
az deployment group create --resource-group <group-name> --template-file .\main.bicep --tags Project=Chewbacca

az webapp deploy --resource-group <group-name> --name <app-name> --src-path <zip-package-path>
az webapp log tail --resource-group <group-name> --name <app-name>
```

## WIP: Creating the Managed Identity SQL User in the pipeline

Why can't things just be easy!

https://stackoverflow.com/questions/53001874/cant-create-azure-sql-database-users-mapped-to-azure-ad-identities-using-servic/56150547#56150547
https://github.com/MicrosoftDocs/azure-docs/issues/52058
https://vosseburchttechblog.azurewebsites.net/index.php/2020/06/12/azure-sql-with-aad-authorization/

```sql
-- type E for AAD User or Service Principal/MSI
CREATE USER [myAADUserName] WITH sid = <sid>, type = E;
```

```powershell
param (
    [string]$objectIdOrAppId
)

[guid]$guid = [System.Guid]::Parse($objectIdOrAppId)

foreach ($byte in $guid.ToByteArray())
{
    $byteGuid += [System.String]::Format("{0:X2}", $byte)
}

return "0x" + $byteGuid
```