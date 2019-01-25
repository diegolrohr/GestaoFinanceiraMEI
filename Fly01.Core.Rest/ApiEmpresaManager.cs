using Fly01.Core.Config;
using Fly01.Core.ViewModels;
using System.Collections.Generic;

namespace Fly01.Core.Rest
{
    public static class ApiEmpresaManager
    {
        public static ManagerEmpresaVM GetEmpresa(string plataformaUrl)
        {
            return RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlManager}", $"company/{plataformaUrl}");
        }
    }
}