using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL
{
    public class SubstituicaoTributariaBL
    {
        public SubstituicaoTributariaBL(AppDataContextBase context)
        {
        }

        public TributacaoRetornoBaseVM CalcularSubstituicaoTributaria(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            //1.Calcular a Base do ICMS Inter ou Base do ICMS Próprio
            //2.Calcular o Valor do ICMS Inter ou Valor do ICMS Próprio
            var valorInterEstadual = CalculaValorInterEstadual(entity, TabelaIcmsBL);
            //3.Calcular a Base do ICMS ST
            var valorIntraEstadual = CalculaValorIntraEstadual(entity, TabelaIcmsBL);

            //4.E por fim… calcular o Valor do ICMS ST:
            //Valor do ICMS ST = (Base do ICMS ST *(Alíquota do ICMS Intra / 100)) -Valor do ICMS Inter
            entity.SubstituicaoTributaria.Valor = Math.Round((valorIntraEstadual - valorInterEstadual), 2);

            return new TributacaoRetornoBaseVM { Base = entity.SubstituicaoTributaria.Base, Aliquota = entity.SubstituicaoTributaria.Aliquota, Valor = entity.SubstituicaoTributaria.Valor, AgregaTotalNota = true };
        }

        private double CalculaValorIntraEstadual(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            var valorBaseIntra = ObterValorBaseIntraEstadual(entity);

            var aliquotaIntraEstadual = entity.SubstituicaoTributaria.AliquotaIntraEstadual > 0 ?
                                        entity.SubstituicaoTributaria.AliquotaIntraEstadual : ObterAliquotaIntraEstadual(entity, TabelaIcmsBL);

            //Base com MVA
            valorBaseIntra += (valorBaseIntra / 100 * entity.SubstituicaoTributaria.Mva);
            //Se tiver redução, pós MVA
            if (entity.SubstituicaoTributaria.PercentualReducaoBCST.HasValue && entity.SubstituicaoTributaria.PercentualReducaoBCST > 0 && (entity.Icms.CSOSN == TipoTributacaoICMS.ComRedDeBaseDeST))
            {
                var reducao = Math.Round(valorBaseIntra / 100 * entity.SubstituicaoTributaria.PercentualReducaoBCST.Value, 2);
                valorBaseIntra -= reducao;
            }

            entity.SubstituicaoTributaria.Aliquota = aliquotaIntraEstadual;
            entity.SubstituicaoTributaria.Base = valorBaseIntra;

            var valorIntraEstadual = Math.Round(entity.SubstituicaoTributaria.Base / 100 * aliquotaIntraEstadual, 2);
            return valorIntraEstadual;
        }

        private double CalculaValorInterEstadual(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            var valorBaseInter = ObterValorBaseInterEstadual(entity);
            var aliquotaInterEstadual = entity.SubstituicaoTributaria.AliquotaInterEstadual > 0 ?
                                        entity.SubstituicaoTributaria.AliquotaInterEstadual : ObterAliquotaInterEstadual(entity, TabelaIcmsBL);

            var valorInterEstadual = Math.Round(valorBaseInter / 100 * aliquotaInterEstadual, 2);
            return valorInterEstadual;
        }

        private double ObterValorBaseInterEstadual(Tributacao entity)
        {
            //Base do ICMS Inter = (Valor do produto + Frete + Seguro + Outras Despesas Acessórias - Descontos)
            var valorBase = entity.ValorBase;
            valorBase += entity.SubstituicaoTributaria.DespesaNaBase ? entity.ValorDespesa : 0;
            valorBase += entity.SubstituicaoTributaria.FreteNaBase ? entity.ValorFrete : 0;
            if (entity.SubstituicaoTributaria.PercentualReducaoBCICMSST.HasValue && entity.SubstituicaoTributaria.PercentualReducaoBCICMSST > 0 && (entity.Icms.CSOSN == TipoTributacaoICMS.ComRedDeBaseDeST))
            {
                var reducao = Math.Round(valorBase / 100 * entity.SubstituicaoTributaria.PercentualReducaoBCICMSST.Value, 2);
                valorBase -= reducao;
            }

            return valorBase;
        }

        private double ObterValorBaseIntraEstadual(Tributacao entity)
        {
            //Base do ICMS ST = (Valor do produto + Valor do IPI + Frete + Seguro + Outras Despesas Acessórias - Descontos) *(1 + (% MVA / 100))
            var valorBase = entity.ValorBase;

            valorBase += entity.SubstituicaoTributaria.DespesaNaBase ? entity.ValorDespesa : 0;
            valorBase += entity.SubstituicaoTributaria.FreteNaBase ? entity.ValorFrete : 0;
            valorBase = entity.SubstituicaoTributaria.IpiNaBase ? valorBase + entity.Ipi.Valor : valorBase;

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
                 : 0.0;
        }

        private double ObterAliquota(string EstadoOrigem, string EstadoDestino, TabelaIcmsBL TabelaIcmsBL)
        {
            return (from e in TabelaIcmsBL.All
                    where e.SiglaOrigem.Equals(EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                    && e.SiglaDestino.Equals(EstadoDestino, StringComparison.InvariantCultureIgnoreCase)
                    select e.IcmsAliquota).FirstOrDefault();
        }
    }
}
