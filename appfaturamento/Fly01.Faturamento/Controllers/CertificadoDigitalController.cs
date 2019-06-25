using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Web.Http.Results;
using Fly01.Core.Rest;
using System.Web.Mvc;
using System;
using Fly01.Core;
using Fly01.Core.Helpers;
using Newtonsoft.Json;

namespace Fly01.Faturamento.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.FaturamentoConfiguracoesCertificadoDigital)]
    public class CertificadoDigitalController : CertificadoDigitalBaseController<CertificadoDigitalVM>
    {
        public CertificadoDigitalController()
            : base() { }

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