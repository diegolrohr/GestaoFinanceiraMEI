using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class FcpBL
    {
        public FcpBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Fcp(Tributacao entity)
        {
            entity.Fcp.Base = entity.Icms.Base;
            
            entity.Fcp.Valor = Math.Round(entity.Fcp.Base / 100 * entity.Fcp.Aliquota,2);

            return new TributacaoRetornoBaseVM { Base = entity.Fcp.Base, Aliquota = entity.Fcp.Aliquota, Valor = entity.Fcp.Valor, AgregaTotalNota = false };
        }
    }
}
