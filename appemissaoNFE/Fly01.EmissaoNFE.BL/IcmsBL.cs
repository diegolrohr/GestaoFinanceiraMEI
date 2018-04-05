using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.EmissaoNFE.Domain.ViewModel;
using Fly01.Core.Domain;
using System;
using System.Linq;

namespace Fly01.EmissaoNFE.BL
{
    public class IcmsBL
    {
        public IcmsBL(AppDataContextBase context)
        {
        }
        
        public TributacaoRetornoBaseVM Icms(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            entity.Icms.Aliquota = (from e in TabelaIcmsBL.All
                                where e.SiglaDestino.Equals(entity.Icms.EstadoDestino, StringComparison.InvariantCultureIgnoreCase)
                                && e.SiglaOrigem.Equals(entity.Icms.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                                select e.IcmsAliquota).FirstOrDefault();

            if(entity.Icms.CSOSN == CSOSN.Outros)
            {
                entity.Icms.Base = entity.Icms.IpiNaBase ? entity.ValorBase + entity.Ipi.Valor : entity.ValorBase;
                entity.Icms.Base += entity.Icms.DespesaNaBase ? entity.ValorDespesa : 0;
                entity.Icms.Base += entity.Icms.FreteNaBase ? entity.ValorFrete : 0;

                entity.Icms.Valor = Math.Round(entity.Icms.Base / 100 * entity.Icms.Aliquota, 2);
            }
            else
            {
                entity.Icms.Base = 0;
                entity.Icms.Valor = 0;
            }
            return new TributacaoRetornoBaseVM { Base = entity.Icms.Base, Aliquota = entity.Icms.Aliquota, Valor = entity.Icms.Valor, AgregaTotalNota = false };
        }
    }
}