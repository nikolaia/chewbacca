<div align="center">
    <img src="docs/logo.png" width="200" height="200">
    <h1>Chewbacca</h1>

</div>

> Dette er en hårete løsning som håndterer mye. Men som Chewbacca fra Star Wars tar den de tunge løftene for vennene  sine (les: andre internapplikasjoner) og er alltid til å stole på.

Variant har mange internesystemer (UniEconomy, Harvest, CVPartner etc.) og denne løsningen fungerer som en proxy og cache for løsninger som ønsker å bruke data fra disse systemene.

Løsningen er bygget slik at arbeid på den gir relevant erfaring for hva vi møter ute hos våre kunder. [Les mer om det her](docs/relevance.md).

Det det er tatt noen avgjørelser rundt arkitektur og hvordan vi bruker skytjenester. De avgjørelsene kan du [lese mer om her](docs/architecture.md).

## Up and running

Løsningen skal fungere lokalt uten noe ekstra oppsett, men alle integrasjoner vil da være _mocked_.

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