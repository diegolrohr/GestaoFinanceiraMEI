using Fly01.Core;
using Fly01.Core.Presentation;
using Fly01.Core.Presentation.Controllers;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels.Presentation.Commons;
using System.Linq;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Fly01.Compras.Controllers
{
    [OperationRole(ResourceKey = ResourceHashConst.ComprasCadastrosCategoria)]
    public class ParametroTributarioController : ParametroTributarioBaseController<ParametroTributarioVM>
    {
        public ParametroTributarioController()
            : base() { }

        public new JsonResult CarregaParametro()
        {
            var parametroTributario = base.GetParametro();

            if (parametroTributario == null)
                return Json(new
                {
                    aliquotaSimplesNacional = "0",
                    aliquotaISS = "5",
                    aliquotaPISPASEP = "0,65",
                    aliquotaCOFINS = "2",
                    numeroRetornoNF = "1",
                    mensagemPadraoNota = "Nota Fiscal.",
                    tipoVersaoNFe = "v4",
                    tipoAmbiente = "Producao",
                    tipoModalidade = "Normal",
                    aliquotaFCP = "0",
                    tipoPresencaComprador = "Presencial",
                    horarioVerao = "Nao",
                    tipoHorario = "Brasilia"
                }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                registroSimplificadoMT = parametroTributario.RegistroSimplificadoMT,
                aliquotaSimplesNacional = parametroTributario.AliquotaSimplesNacional,
                aliquotaISS = parametroTributario.AliquotaISS,
                aliquotaPISPASEP = parametroTributario.AliquotaPISPASEP,
                aliquotaCOFINS = parametroTributario.AliquotaCOFINS,
                numeroRetornoNF = parametroTributario.NumeroRetornoNF,
                tipoModalidade = parametroTributario.TipoModalidade,
                tipoVersaoNFe = parametroTributario.TipoVersaoNFe,
                mensagemPadraoNota = parametroTributario.MensagemPadraoNota,
                tipoAmbiente = parametroTributario.TipoAmbiente,
                aliquotaFCP = parametroTributario.AliquotaFCP,
                tipoPresencaComprador = parametroTributario.TipoPresencaComprador,
                horarioVerao = parametroTributario.HorarioVerao,
                tipoHorario = parametroTributario.TipoHorario
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
