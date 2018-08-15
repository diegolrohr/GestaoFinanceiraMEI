using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class ConcreteTransmissaoFactory : TransmissaoFactory
    {
        public override TransmissaoVM ObterTransmissao(NFe entity, TransmissaoBLs transmissaoBLs)
        {
            switch (entity.TipoVenda)
            {
                case Core.Entities.Domains.Enum.TipoVenda.Normal:
                    var normal = new TransmissaoNFeNormal(entity, transmissaoBLs);
                    return normal.ObterTransmissaoVM();                    
                case Core.Entities.Domains.Enum.TipoVenda.Complementar:
                    return TransmissaoTipoComplementar(entity, transmissaoBLs);
                case Core.Entities.Domains.Enum.TipoVenda.Devolucao:
                    var devolucao = new TransmissaoNFeDevolucao(entity, transmissaoBLs);
                    return devolucao.ObterTransmissaoVM();
                default:
                    return null;
            }
        }

        private TransmissaoVM TransmissaoTipoComplementar(NFe entity, TransmissaoBLs transmissaoBLs)
        {
            switch (entity.TipoNfeComplementar)
            {
                case Core.Entities.Domains.Enum.TipoNfeComplementar.ComplPrecoQtd:
                    var complementarPreco = new TransmissaoNFeComplementoPreco(entity, transmissaoBLs);
                    return complementarPreco.ObterTransmissaoVM();
                case Core.Entities.Domains.Enum.TipoNfeComplementar.ComplIcms:
                    var complementarIcms = new TransmissaoNFeComplementoICMS(entity, transmissaoBLs);
                    return complementarIcms.ObterTransmissaoVM();
                default:
                    return null;
            }
        }
    }
}
