# ibas-menu

```bash
 $ export RESGRP=IBASMenuRG
 $ export WebAppName=ibas-menu-webapp-26375-Dynamic
 $ az webapp connection list -g $RESGRP -n $WebAppName --output table
```

```bash
 $ dotnet add package Azure.Identity
 $ dotnet add package Azure.Data.Tables
```
