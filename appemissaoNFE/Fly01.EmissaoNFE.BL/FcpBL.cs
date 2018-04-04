﻿using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core;
using System;
using System.Linq;

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
