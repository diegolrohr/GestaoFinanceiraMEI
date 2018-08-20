using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoICMSST : TransmissaoNFe
    {
        public TransmissaoNFeComplementoICMSST(NFe nfe, TransmissaoBLs transmissaoBLs) 
            : base(nfe, transmissaoBLs) {}

        public override TipoNota ObterTipoDocumentoFiscal()
        {
            throw new NotImplementedException();
        }

        public override TipoFormaPagamento ObterTipoFormaPagamento()
        {
            throw new NotImplementedException();
        }

        public override TransmissaoVM ObterTransmissaoVM()
        {            
            throw new NotImplementedException();
        }

        public override bool PagaFrete()
        {
            throw new NotImplementedException();
        }
    }
}
