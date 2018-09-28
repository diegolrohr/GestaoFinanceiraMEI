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
            var valorBase = entity.ValorBase + (entity.Cofins.FreteNaBase ? entity.ValorFrete : 0);
            return new TributacaoRetornoBaseVM
            {
                Base = valorBase,
                Aliquota = entity.Cofins.Aliquota,
                Valor = Math.Round(valorBase / 100 * entity.Cofins.Aliquota, 2),
                AgregaTotalNota = false
            };
        }
    }
}
