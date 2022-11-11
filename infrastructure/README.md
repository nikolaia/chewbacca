# WIP

```az
az group create -l westeurope -n <group-name> --tags Project=Chewbacca 
az deployment group create --resource-group <group-name> --template-file .\main.bicep --tags Project=Chewbacca

az webapp deploy --resource-group <group-name> --name <app-name> --src-path <zip-package-path>
az webapp log tail --resource-group <group-name> --name <app-name>
```