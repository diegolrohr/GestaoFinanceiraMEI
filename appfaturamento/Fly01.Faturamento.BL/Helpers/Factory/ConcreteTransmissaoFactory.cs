using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
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
                case TipoVenda.Normal:
                    var normal = new TransmissaoNFeNormal(entity, transmissaoBLs);
                    return normal.ObterTransmissaoVM();                    
                case TipoVenda.Complementar:
                    return TransmissaoTipoComplementar(entity, transmissaoBLs);
                case TipoVenda.Devolucao:
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
                case TipoNfeComplementar.ComplPrecoQtd:
                    var complementarPreco = new TransmissaoNFeComplementoPreco(entity, transmissaoBLs);
                    return complementarPreco.ObterTransmissaoVM();
                case TipoNfeComplementar.ComplIcms:
                    var complementarIcms = new TransmissaoNFeComplementoICMS(entity, transmissaoBLs);
                    return complementarIcms.ObterTransmissaoVM();
                case TipoNfeComplementar.ComplIcmsST:
                    var complementarIcmsST = new TransmissaoNFeComplementoICMSST(entity, transmissaoBLs);
                    return complementarIcmsST.ObterTransmissaoVM();
                case TipoNfeComplementar.ComplIpi:
                    var complementarIpi = new TransmissaoNFeComplementoIPI(entity, transmissaoBLs);
                    return complementarIpi.ObterTransmissaoVM();
                default:
                    return null;
            }
        }
    }
}
