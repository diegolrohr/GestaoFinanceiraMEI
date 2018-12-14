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
            entity.Fail(string.IsNullOrWhiteSpace(entity.ValorAntecipado), ValorAntecipadoNull);
            entity.Fail(string.IsNullOrWhiteSpace(entity.ValorRecebido), ValorRecebidoNull);
            entity.Fail(string.IsNullOrWhiteSpace(entity.Taxa), TaxaNull);
        }

        public static Error ValorAntecipadoNull = new Error("Valor Antecipado Nulo.");
        public static Error ValorRecebidoNull = new Error("Valor Recebido Nulo.");
        public static Error TaxaNull = new Error("Taxa Nulo.");
    }
}
