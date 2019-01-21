using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;

namespace Fly01.Financeiro.BL
{
    public class StoneAntecipacaoRecebiveisBL : PlataformaBaseBL<StoneAntecipacaoRecebiveis>
    {
        public StoneAntecipacaoRecebiveisBL(AppDataContextBase context) : base(context)
        { }

        public override void ValidaModel(StoneAntecipacaoRecebiveis entity)
        {
            entity.Fail(entity.ValorAntecipado <= 0, new Error("Valor antecipado deve ser superior a zero.", "valorAntecipado"));
            entity.Fail(entity.ValorBruto <= 0, new Error("Valor recebido deve ser superior a zero.", "valorBruto"));
            entity.Fail(entity.TaxaPontual <= 0, new Error("Valor da taxa deve ser superior a zero.", "taxaPontual"));
            entity.Fail(entity.StoneBancoId <= 0, new Error("Stone bancoId inválido.", "stoneBancoId"));
            entity.Fail(entity.StoneId <= 0, new Error("Stone id inválido.", "StoneId"));
        }
    }
}
