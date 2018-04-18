using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class SubstituicaoTributariaBL
    {
        public SubstituicaoTributariaBL(AppDataContextBase context)
        {
        }

        public TributacaoRetornoBaseVM SubstituicaoTributaria(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            entity.SubstituicaoTributaria.Base = entity.SubstituicaoTributaria.IpiNaBase ? entity.ValorBase + entity.Ipi.Valor : entity.ValorBase;
            entity.SubstituicaoTributaria.Base += entity.SubstituicaoTributaria.DespesaNaBase ? entity.ValorDespesa : 0;
            entity.SubstituicaoTributaria.Base += entity.SubstituicaoTributaria.FreteNaBase ? entity.ValorFrete : 0;
            
            entity.SubstituicaoTributaria.Aliquota = (from e in TabelaIcmsBL.All
                               where e.SiglaDestino.Equals(entity.SubstituicaoTributaria.EstadoDestino, StringComparison.InvariantCultureIgnoreCase)
                               && e.SiglaOrigem.Equals(entity.SubstituicaoTributaria.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                               select e.IcmsAliquota).FirstOrDefault();

            entity.SubstituicaoTributaria.Base = (100 + entity.SubstituicaoTributaria.Mva) / 100 * entity.SubstituicaoTributaria.Base;
            entity.SubstituicaoTributaria.Valor = Math.Round(entity.SubstituicaoTributaria.Base / 100 * entity.SubstituicaoTributaria.Aliquota, 2);

            if(entity.FcpSt != null)
            {

            }
            return new TributacaoRetornoBaseVM { Base = entity.SubstituicaoTributaria.Base, Aliquota = entity.SubstituicaoTributaria.Aliquota, Valor = entity.SubstituicaoTributaria.Valor, AgregaTotalNota = true };
        }
    }
}
