using Newtonsoft.Json;
using System;
using System.IO;
using System.Web.Http;

namespace Fly01.Core.API
{
    public abstract class ApiHelloBaseController : ApiBaseController
    {
        protected string DllFileName { get; set; }
        protected string PathDll { get; set; }

        public ApiHelloBaseController(string pathDll, string dllFileName)
        {
            PathDll = pathDll;
            DllFileName = dllFileName;
        }

        private string GetDateLastCompile()
        {
            var fi = new FileInfo(string.Concat(PathDll, DllFileName));
            return String.Format("{0:00} {1:00}h{2:00}", fi.LastWriteTime.ToString("dd/MM/yyyy"), fi.LastWriteTime.Hour, fi.LastWriteTime.Minute);
        }

        public abstract void TestDbConnection();

        [HttpGet]
        public IHttpActionResult Say()
        {
            Request.Headers.Add("EmpresaId", Guid.Empty.ToString());
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
                ApplicationName = DllFileName,
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