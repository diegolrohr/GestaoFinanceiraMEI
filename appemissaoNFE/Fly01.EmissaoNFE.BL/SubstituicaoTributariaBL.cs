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

        public TributacaoRetornoBaseVM CalcularSubstituicaoTributaria(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            var valorBase = ObterValorBase(entity);

            var aliquotaIntraEstadual = ObterAliquotaIntraEstadual(entity, TabelaIcmsBL);
            var aliquotaInterEstadual = ObterAliquotaInterEstadual(entity, TabelaIcmsBL);

            entity.SubstituicaoTributaria.Aliquota = aliquotaIntraEstadual;
            entity.SubstituicaoTributaria.Base = (100 + entity.SubstituicaoTributaria.Mva) / 100 * valorBase;

            var valorIntraEstadual = Math.Round(entity.SubstituicaoTributaria.Base / 100 * aliquotaIntraEstadual, 2);
            var valorInterEstadual = Math.Round(valorBase / 100 * aliquotaInterEstadual, 2);

            entity.SubstituicaoTributaria.Valor = valorIntraEstadual - valorInterEstadual;

            return new TributacaoRetornoBaseVM { Base = entity.SubstituicaoTributaria.Base, Aliquota = entity.SubstituicaoTributaria.Aliquota, Valor = entity.SubstituicaoTributaria.Valor, AgregaTotalNota = true };
        }

        private double ObterValorBase(Tributacao entity)
        {
            var valorBase = entity.SubstituicaoTributaria.IpiNaBase ? entity.ValorBase + entity.Ipi.Valor : entity.ValorBase;
            valorBase += entity.SubstituicaoTributaria.DespesaNaBase ? entity.ValorDespesa : 0;
            valorBase += entity.SubstituicaoTributaria.FreteNaBase ? entity.ValorFrete : 0;

            return valorBase;
        }

        private double ObterAliquotaIntraEstadual(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            return ObterAliquota(entity.SubstituicaoTributaria.EstadoOrigem, entity.SubstituicaoTributaria.EstadoOrigem, TabelaIcmsBL);
        }

        private double ObterAliquotaInterEstadual(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            return entity.SubstituicaoTributaria.EstadoOrigem != entity.SubstituicaoTributaria.EstadoDestino 
                ? ObterAliquota(entity.SubstituicaoTributaria.EstadoOrigem, entity.SubstituicaoTributaria.EstadoDestino, TabelaIcmsBL)
                 :0.0;
        }

        private double ObterAliquota(string EstadoOrigem, string EstadoDestino, TabelaIcmsBL TabelaIcmsBL)
        {
            return  (from e in TabelaIcmsBL.All
                        where e.SiglaDestino.Equals(EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                        && e.SiglaOrigem.Equals(EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                        select e.IcmsAliquota).FirstOrDefault();
        }
    }
}
