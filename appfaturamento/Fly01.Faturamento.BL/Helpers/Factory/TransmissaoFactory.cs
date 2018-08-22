using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public abstract class TransmissaoFactory
    {
        public abstract TransmissaoVM ObterTransmissao(NFe entity, TransmissaoBLs transmissaoBLs);
    }
}
