# Howto

Integration med Azure Tables.

## 1. Tilføj nuget

```bash
dotnet add package Azure.Data.Tables
```

---

## 2. MenuItemDTO nedarver fra ITableEntity

Udvid klassen `MenuItemDTO` i filen `Model/MenuItemDTO.cs` således at den nedarver fra ITabelEntity.
Så er den nemmere at transformere efter en Query i en Azure Tabel.

---

## 3.  Opret en Azure Tabel til menuen

Start med at lave en Storage Account, hvis du ikke har én i forvejen.

```bash

export STORAGEACCNAME=ibaskantinestorage7788
export RESGRPNAME=IBASMenuRG
export LOCATION=northeurope

az storage account create \
--name $STORAGEACCNAME \
--resource-group $RESGRPNAME \
--location $LOCATION \
--sku Standard_LRS

```

Opret en MenuTabel og indsæt data for en uge.

---

## 4. Tilføj Forbindelse til tabelen

- Set miljøvariable `AzureStorage` til din Storage Accounts connectionString (bemærk 2 x `_`):

```bash
 export export ConnectionStrings__AzureStorage="DefaultEndpointsProtocol=https;AccountName=ibaskantinestorage1234;AccountKey=<mangler her !!!>;EndpointSuffix=core.windows.net"
```

- I Constructoren af klassen `IndexModel` indlæs miljøvariablen `AzureStorage` til en variabel og opret forbindelse til din `MenuTabel`:

```csharp
private readonly ILogger<IndexModel> _logger;
private TableClient? _tableClient;

public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
{
    _logger = logger;

    var connectionString = config.GetConnectionString("AzureStorage");

    if (string.IsNullOrEmpty(connectionString))
    {
        logger.LogCritical("Connection string is empty.");
        return;
    }

    try {
        this._tableClient = new TableClient(
            connectionString,
            "MenuTable"
        );
    } catch (Exception ex)
    {
        logger.LogCritical(ex, "Could not connect to storage.");
    }
}
```

---

## 5. Tilføj en liste til query-svar

På klasse-niveau, opret en `Pageable`-liste til at indeholde det data som kommer fra databasen:

``` csharp
    public Pageable<MenuItemDTO> MenuItems {get; set;}
```

---

## 6. Implementér OnGet()

Tilføj koden til OnGet-metoden som kan hente data fra Menutabellen og placere den i klassens `MenuItemDTO` liste:

```csharp
public void OnGet()
{
    if (this._tableClient == null)
    {
        return;
    }
    this.MenuItems = _tableClient.Query<MenuItemDTO>();
}
```

---

## 7. Ny web app til dynamicmenu branch

Opret en ny webapp i den eksisterende plan, og tilknyt den til den nye branch.

``` bash
export RESGRP=IBASMenuRG
export WEBAPPPLAN=ibas-menu-webapp-26375
export WEBAPPNAME=ibas-menu-webapp-26375-Dynamic
export GITHUBREPO="https://github.com/HNRK-JNSN/ibas-menu.git"

az webapp create --name $WEBAPPNAME --resource-group $RESGRP \
--plan $WEBAPPPLAN --runtime "DOTNETCORE:8.0"

az webapp deployment source config \
--name $WEBAPPNAME --resource-group $RESGRP \
--repository-type github --repo-url  $GITHUBREPO \
--branch dynamicmenu
```

---

# 8. Tilføj ConnectionString 

Find nu den nye WebApp i portalen og under `Settings`-->`Environment variables` find fanebladet `Connection strings` og vælg `+ Add`-knappen øverst.
Den connectionString som vi skal bruge i koden skal navngives `AzureStorage` og værdien er selvfølgig den connectionString som passer til din konto.
Sæt den til typen `Custom`fra dropdown menuen.

Vælg `Apply`og `Apply`igen for at gemme miljø variablen.

---

# 9. Afprøv den ny app.

---

# 10. Service Connector

Ønsker du at oprette en service connector kan du køre følgende kommando:

```bash
 az webapp connection create storage-table -g $RESGRP -n $WEBAPPNAME --tg $RESGRP --account ibaskantinestorage7788 --system-identity
```

Hint: læs artiklen [](https://learn.microsoft.com/en-us/azure/service-connector/how-to-integrate-storage-table?tabs=dotnet)
