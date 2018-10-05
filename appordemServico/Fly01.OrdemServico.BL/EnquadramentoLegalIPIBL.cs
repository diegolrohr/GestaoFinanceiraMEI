using Fly01.OrdemServico.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.OrdemServico.BL
{
    public class EnquadramentoLegalIPIBL : DomainBaseBL<EnquadramentoLegalIPI>
    {
        public EnquadramentoLegalIPIBL(AppDataContext context) : base(context) { }
    }
}
