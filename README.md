![N|Solid](https://cdnfly01.azureedge.net/img/fly01logopreto.png)

# Fly01.Apps

Fly01.Apps é a consolidação em uma única solução dos seguintes projeto Fly01:
  * Core
    ```sh
        Fly01.Core
        Fly01.Core.Helpers
        Fly01.Core.Notifications
        Fly01.Core.SOAManager
  * Domain
    ```sh
        Fly01.Compras.Domain
        Fly01.EmissaoNFE.Domain
        Fly01.Estoque.Domain
        Fly01.Faturamento.Domain
        Fly01.Financeiro.Domain
  * Infrastructure
    ```sh
        Fly01.Compras.BL
        Fly01.Compras.DAL
        Fly01.EmissaoNFE.BL
        Fly01.EmissaoNFE.DAL
        Fly01.Estoque.BL
        Fly01.Estoque.DAL
        Fly01.Faturamento.BL
        Fly01.Faturamento.DAL
        Fly01.Financeiro.BL
        Fly01.Financeiro.DAL
  * Presentation
    ```sh
        Fly01.Compras
        Fly01.Compras.Entities
        Fly01.Estoque
        Fly01.Estoque.Entities
        Fly01.Faturamento
        Fly01.Faturamento.Entities
        Fly01.Financeiro
        Fly01.Financeiro.Entities
  * Service
    ```sh
        Fly01.Compras.API
        Fly01.EmissaoNFE.API
        Fly01.Estoque.API
        Fly01.Faturamento.API
        Fly01.Financeiro.API
#
# Packages necessários por projeto.

  - .Domain
    ```sh
        Install-Package Newtonsoft.Json -Version 11.0.2
  - .DAL
    ```sh
        Install-Package EntityFramework -Version 6.2.0
  - .BL
    ```sh
        Install-Package EntityFramework -Version 6.2.0
        Install-Package Newtonsoft.Json -Version 11.0.2
  - .Entities
    ```sh
        Install-Package Newtonsoft.Json -Version 11.0.2
  - .<ProjectName>
    ```sh
        Install-Package Fly01.uiJS -Version 2.0.1.1
        Install-Package Microsoft.AspNet.Mvc -Version 5.2.4
        Install-Package Microsoft.AspNet.Web.Optimization -Version 1.1.3
        Install-Package Microsoft.AspNet.WebApi -Version 5.2.4
        Install-Package MicrosoftReportViewerWebForms -Version 11.0.0.0
        Install-Package Newtonsoft.Json -Version 11.0.2
  - .API
    ```sh
        Install-Package EntityFramework -Version 6.2.0
        Install-Package Microsoft.AspNet.Cors -Version 5.2.4
        Install-Package Microsoft.AspNet.Mvc -Version 5.2.4
        Install-Package Microsoft.AspNet.OData -Version 6.1.0
        Install-Package Microsoft.AspNet.WebApi.Client -Version 5.2.4
        Install-Package Microsoft.AspNet.WebApi.Cors -Version 5.2.4
        Install-Package Microsoft.AspNet.WebApi.Owin -Version 5.2.4
        Install-Package Microsoft.AspNet.WebApi.WebHost -Version 5.2.4
        Install-Package Microsoft.OData.Core -Version 7.4.3
        Install-Package Microsoft.Owin -Version 4.0.0
        Install-Package Microsoft.Owin.Cors -Version 4.0.0
        Install-Package Newtonsoft.Json -Version 11.0.2
