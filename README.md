<div align="center">
    <img src="docs/logo.png" width="200" height="200">
    <h1>Chewbacca</h1>

</div>

> Dette er en hårete løsning som håndterer mye. Men som Chewbacca fra Star Wars tar den de tunge løftene for vennene  sine (les: andre internapplikasjoner) og er alltid til å stole på.

Variant har mange internesystemer (UniEconomy, Harvest, CVPartner etc.) og denne løsningen fungerer som en proxy og cache for løsninger som ønsker å bruke data fra disse systemene.

Løsningen er bygget slik at arbeid på den gir relevant erfaring for hva vi møter ute hos våre kunder. [Les mer om det her](docs/relevance.md).

Det det er tatt noen avgjørelser rundt arkitektur og hvordan vi bruker skytjenester. De avgjørelsene kan du [lese mer om her](docs/architecture.md).

## Up and running med mocked data

Løsningen skal fungere lokalt uten noe ekstra oppsett, men alle integrasjoner vil da være _mocked_.

Installer Docker for Windows/Mac fra dockers hjemmeside. Kjør opp SQL Server og installer entitiy framework tools:

```bash
# Installer tools for entity framework (ORMen)
dotnet tool install --global dotnet-ef

# Kjør opp en lokal database i docker
docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=yourStrong(!)Password' -p 1433:1433 --name azuresqledge -d mcr.microsoft.com/azure-sql-edge
```

Man kan da kjøre migrations fra cmd line:

```bash
dotnet ef database update -s .\src\WebApi\ -p .\src\Database\
```

## Up and running med ekte integrasjoner

For å få tilgang til integrasjoner må man ha: 

1. En variant-bruker/epost
2. Blitt lagt til i _developers_ gruppen i Azure AD
3. Installert _Azure CLI_ og kjørt `az login`

Man kan da få konfigurasjon og secrets fra azure uten noe ekstra oppsett.

## Infrastructure

Work in progress. Se i `infrastructure`-mappa

```bash
az deployment group create --resource-group my-test-group --template-file .\infrastructure\azuredeploy.bicep --location westeurope`
```