using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class IssBL
    {
        public IssBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Iss(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.Iss.Aliquota,
                Valor = entity.Iss.CalculaIss ? Math.Round(entity.ValorBase / 100 * entity.Iss.Aliquota, 2) : 0,
                ValorRetencao = entity.Iss.RetemIss ? Math.Round(entity.ValorBase / 100 * entity.Iss.Aliquota, 2) : 0,
                AgregaTotalNota = false
            };
        }
    }
}
