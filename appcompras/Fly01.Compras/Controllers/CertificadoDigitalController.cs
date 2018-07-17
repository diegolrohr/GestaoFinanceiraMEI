using Fly01.Core.Presentation.Controllers;
using Fly01.Faturamento.ViewModel;

namespace Fly01.Compras.Controllers
{
    public class CertificadoDigitalController : CertificadoDigitalBaseController<CertificadoDigitalVM>
    {
        public CertificadoDigitalController()
            : base(/*ResourceHashConst.FaturamentoConfiguracoesCertificadoDigital*/) { }

    }
}