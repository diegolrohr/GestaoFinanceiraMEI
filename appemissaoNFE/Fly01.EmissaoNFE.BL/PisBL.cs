using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class PisBL
    {
        public PisBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Pis(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.Pis.Aliquota,
                Valor = Math.Round(entity.ValorBase / 100 * entity.Pis.Aliquota, 2),
                AgregaTotalNota = false
            };
        }
    }
}
