using Fly01.Core.Helpers;
using Fly01.Core.ViewModels;

namespace Fly01.Core.Rest
{
    public static class ApiEmpresaManager
    {
        public static ManagerEmpresaVM GetEmpresa(string plataformaUrl)
        {
            return RestHelper.ExecuteGetRequest<ResponseDataVM<ManagerEmpresaVM>>($"{AppDefaults.UrlManager}", $"company/{plataformaUrl}")?.Data;
        }
    }
}