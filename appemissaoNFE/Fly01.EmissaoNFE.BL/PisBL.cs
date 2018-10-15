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
            var valorBase = entity.ValorBase + (entity.Pis.FreteNaBase ? entity.ValorFrete : 0);
            return new TributacaoRetornoBaseVM
            {
                Base = valorBase,
                Aliquota = entity.Pis.Aliquota,
                Valor = entity.Pis.CalculaPis ? Math.Round(valorBase / 100 * entity.Pis.Aliquota, 2) : 0,
                ValorRetencao = entity.Pis.RetemPis ? Math.Round(valorBase / 100 * entity.Pis.Aliquota, 2) : 0,
                AgregaTotalNota = false
            };
        }
    }
}
