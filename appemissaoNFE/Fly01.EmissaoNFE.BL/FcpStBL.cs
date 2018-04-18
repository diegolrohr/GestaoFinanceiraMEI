using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class FcpStBL
    {
        public FcpStBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM FcpSt(Tributacao entity)
        {
            entity.FcpSt.Base = entity.SubstituicaoTributaria.Base;
            
            entity.FcpSt.Valor = Math.Round(entity.FcpSt.Base / 100 * entity.FcpSt.Aliquota,2);

            return new TributacaoRetornoBaseVM { Base = entity.FcpSt.Base, Aliquota = entity.FcpSt.Aliquota, Valor = entity.FcpSt.Valor, AgregaTotalNota = true };
        }
    }
}
