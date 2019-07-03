using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.EmissaoNFE.BL
{
    public class IcmsBL
    {
        public IcmsBL(AppDataContextBase context)
        {
        }

        public TributacaoRetornoBaseVM Icms(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            if (!entity.Icms.Aliquota.HasValue)
            {
                entity.Icms.Aliquota = (from e in TabelaIcmsBL.All
                                        where e.SiglaDestino.Equals(entity.Icms.EstadoDestino, StringComparison.InvariantCultureIgnoreCase)
                                        && e.SiglaOrigem.Equals(entity.Icms.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                                        select e.IcmsAliquota).FirstOrDefault();
            }

            entity.Icms.Base = entity.ValorBase;

            if (entity.Icms.PercentualReducaoBC.HasValue && entity.Icms.PercentualReducaoBC > 0 && (entity.Icms.CSOSN == TipoTributacaoICMS.ComRedDeBaseDeST || entity.Icms.CSOSN == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo))
            {
                var reducao = Math.Round(entity.Icms.Base / 100 * entity.Icms.PercentualReducaoBC.Value, 2);
                entity.Icms.Base -= reducao;
            }

            if (entity.Icms.CSOSN == TipoTributacaoICMS.Outros || entity.Icms.CSOSN == TipoTributacaoICMS.TributadaIntegralmente || entity.Icms.CSOSN == TipoTributacaoICMS.ComRedDeBaseDeST || entity.Icms.CSOSN == TipoTributacaoICMS.ComReducaoDeBaseDeCalculo || entity.Icms.CSOSN == TipoTributacaoICMS.Diferimento || entity.Icms.CSOSN == TipoTributacaoICMS.Outros90 || entity.Icms.CSOSN == TipoTributacaoICMS.TributadaComCobrancaDeSubstituicao)
            {
                entity.Icms.Base = entity.Icms.IpiNaBase ? entity.Icms.Base + entity.Ipi.Valor : entity.Icms.Base;
                entity.Icms.Base += entity.Icms.DespesaNaBase ? entity.ValorDespesa : 0;
                entity.Icms.Base += entity.Icms.FreteNaBase ? entity.ValorFrete : 0;

                entity.Icms.Valor = Math.Round(entity.Icms.Base / 100 * entity.Icms.Aliquota.Value, 2);
            }
            else
            {
                entity.Icms.Base = 0;
                entity.Icms.Valor = 0;
            }
            return new TributacaoRetornoBaseVM { Base = entity.Icms.Base, Aliquota = entity.Icms.Aliquota.Value, Valor = entity.Icms.Valor, AgregaTotalNota = false };
        }
    }
}