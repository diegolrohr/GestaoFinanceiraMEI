﻿using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class ImpostoRendaBL
    {
        public ImpostoRendaBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM ImpostoRenda(Tributacao entity)
        {            
            return new TributacaoRetornoBaseVM
            {
                Base = entity.ValorBase,
                Aliquota = entity.ImpostoRenda.Aliquota,
                Valor = Math.Round(entity.ValorBase / 100 * entity.ImpostoRenda.Aliquota, 2),
                AgregaTotalNota = false
            };
        }
    }
}