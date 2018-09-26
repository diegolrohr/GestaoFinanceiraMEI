using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class CofinsBL
    {
        public CofinsBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Cofins(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.Cofins.Aliquota,
                Valor = Math.Round(entity.ValorBase / 100 * entity.Cofins.Aliquota, 2),
                AgregaTotalNota = false
            };
        }
    }
}
