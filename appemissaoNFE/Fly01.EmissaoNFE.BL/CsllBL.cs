using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class CsllBL
    {
        public CsllBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Csll(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.Csll.Aliquota,
                Valor = Math.Round(entity.ValorBase / 100 * entity.Csll.Aliquota, 2),
                ValorRetencao = entity.Csll.AplicaRetencao ? Math.Round(entity.ValorBase / 100 * entity.Csll.Aliquota, 2) : 0,
                AgregaTotalNota = false
            };
        }
    }
}
