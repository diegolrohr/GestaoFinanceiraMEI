using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Presentation;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Web.Http.Results;
using Fly01.Core.Rest;
using System.Web.Mvc;
using System;
using Fly01.Core;

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
                var response = RestHelper.ExecutePostRequest($"{AppDefaults.UrlGateway}CertificadoDigital/","removecertificados", "", null, 150);
                //ExecutePutRequest<ManagerEmpresaVM>($"{AppDefaults.UrlManager}company/{SessionManager.Current.UserData.PlatformUrl}", empresa, AppDefaults.GetQueryStringDefault());
                return Json(new
                {
                    success = true,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}