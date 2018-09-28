using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class InssBL
    {
        public InssBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Inss(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.Inss.Aliquota,
                Valor = Math.Round(entity.ValorBase / 100 * entity.Inss.Aliquota, 2),
                AgregaTotalNota = false
            };
        }
    }
}
