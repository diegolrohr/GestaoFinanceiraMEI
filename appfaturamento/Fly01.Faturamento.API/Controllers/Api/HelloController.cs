using Fly01.Core.API;
using Fly01.Faturamento.BL;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Web.Http;

namespace Fly01.Faturamento.API.Controllers.Api
{
    public class HelloController : ApiBaseController
    {
        private const string DllName = "Fly01.Faturamento.API.dll";

        private static Lazy<string> DataGatewayDll = new Lazy<string>(() => {
            var fi = new FileInfo(string.Concat(System.Web.HttpRuntime.BinDirectory, DllName));
            return String.Format("{0:00} {1:00}h{2:00}", fi.LastWriteTime.ToString("dd/MM/yyyy"), fi.LastWriteTime.Hour, fi.LastWriteTime.Minute);
        });

        [HttpGet]
        public IHttpActionResult Index()
        {
            Request.Headers.Add("PlataformaUrl", "SayHello");
            Request.Headers.Add("AppUser", "AppUserHello");

            var testConnectionDB = false;
            string errorTestConnection = string.Empty;
            try
            {
                using (UnitOfWork unitOfWork = new UnitOfWork(ContextInitialize))
                {
                    var estados = unitOfWork.EstadoBL.All.ToArray();
                    testConnectionDB = true;
                }
            }
            catch (Exception ex)
            {
                errorTestConnection = ex.Message;
            }

            var dataEnvironment = new
            {
                ApplicationName = DllName,
                LastUpdate = DataGatewayDll.Value,
                DataBase = new
                {
                    IsOnline = testConnectionDB,
                    ErrorTestConnection = errorTestConnection
                }
            };

            return Json(dataEnvironment, new JsonSerializerSettings() { Formatting = Formatting.Indented });
        }
    }
}
