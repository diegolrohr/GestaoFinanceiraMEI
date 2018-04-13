using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Http;

namespace Fly01.Core.API
{
    public abstract class ApiHelloBaseController : ApiBaseController
    {
        private string dllFileName = string.Empty;
        private string pathDll = string.Empty;

        public ApiHelloBaseController(string pathDll, string dllFileName)
        {
            this.pathDll = pathDll;
            this.dllFileName = dllFileName;
        }

        private string GetDateLastCompile()
        {
            var fi = new FileInfo(string.Concat(pathDll, dllFileName));
            return String.Format("{0:00} {1:00}h{2:00}", fi.LastWriteTime.ToString("dd/MM/yyyy"), fi.LastWriteTime.Hour, fi.LastWriteTime.Minute);
        }

        public abstract void TestDbConnection();

        [HttpGet]
        public IHttpActionResult Say()
        {
            Request.Headers.Add("PlataformaUrl", "SayHello");
            Request.Headers.Add("AppUser", "AppUserHello");

            var testConnectionDB = false;
            string errorTestConnection = string.Empty;
            try
            {
                TestDbConnection();
                testConnectionDB = true;
            }
            catch (Exception ex)
            {
                errorTestConnection = ex.Message;
            }

            var dataEnvironment = new
            {
                ApplicationName = dllFileName,
                LastUpdate = GetDateLastCompile(),
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