using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using Fly01.Faturamento.BL.Helpers.EntitiesBL;

namespace Fly01.Faturamento.BL.Helpers.Factory
{
    public class TransmissaoNFeComplementoPreco: TransmissaoNFe
    {
        public TransmissaoNFeComplementoPreco(NFe nfe, TransmissaoBLs transmissaoBLs) 
            : base(nfe, transmissaoBLs) {}

        public override TransmissaoVM ObterTransmissaoVM()
        {
            throw new NotImplementedException();
        }
    }
}
