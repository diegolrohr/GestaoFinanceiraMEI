using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoCobrancaBL : PlataformaBaseBL<NFeImportacaoCobranca>
    {
        public NFeImportacaoCobrancaBL(AppDataContext context) : base(context)
        {
        }

        public override void ValidaModel(NFeImportacaoCobranca entity)
        {
            entity.Fail(entity.Valor <= 0, new Error("Valor deve ser superior a zero", "valor"));
            base.ValidaModel(entity);
        }
    }
}