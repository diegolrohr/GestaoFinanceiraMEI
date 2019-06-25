using Fly01.Core.Helpers;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasConfiguracoesCertificadoDigital)]
    public class CertificadoDigitalController : CertificadoDigitalBaseController<CertificadoDigitalVM>
    {
        public JsonResult RemoveCertificados()
        {
            try
            {

                var response = RestHelper.ExecutePostRequest("removeCertificado", "", null, 150);

                return Json(new
                {
                    success = true,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorInfo error = JsonConvert.DeserializeObject<ErrorInfo>(ex.Message);
                var result = new
                {
                    success = false,
                    message = error.Message
                };

                return Json(result, JsonRequestBehavior.AllowGet); ;

                //throw new Exception(ex.Message);
            }
        }
    }
}