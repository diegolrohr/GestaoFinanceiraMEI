using System;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeDevolucao : TransmissaoNFe
    {
        public TransmissaoNFeDevolucao(NFe nfe, TransmissaoBLs transmissaoBLs) 
            : base(nfe, transmissaoBLs) { }

        public override TipoNota ObterTipoDocumentoFiscal()
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
