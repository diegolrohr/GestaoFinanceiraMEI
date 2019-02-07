using Fly01.Core.Config;
using Fly01.Core.Helpers;
using Fly01.Core.ViewModels;
using System.Collections.Generic;

namespace Fly01.Core.Rest
{
    public static class ApiEmpresaManager
    {
        public static ManagerEmpresaVM GetEmpresa(string plataformaUrl)
        {
            //return RestHelper.ExecuteGetRequest<ResponseDataVM<ManagerEmpresaVM>>($"{AppDefaults.UrlManager}", $"company/{plataformaUrl}")?.Data;
            return RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{plataformaUrl}");
        }
    }
}