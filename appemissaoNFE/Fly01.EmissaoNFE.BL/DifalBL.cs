using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.EmissaoNFE.BL
{
    public class DifalBL
    {
        public DifalBL(AppDataContextBase context)
        {
        }

        public DifalRetornoVM Difal(Tributacao entity, TabelaIcmsBL TabelaIcmsBL)
        {
            double aliquotaIntraestadual = (from e in TabelaIcmsBL.All
                                            where e.SiglaDestino.Equals(entity.Icms.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                               && e.SiglaOrigem.Equals(entity.Icms.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                               select e.IcmsAliquota).FirstOrDefault();

            double aliquotaInterestadual = (from e in TabelaIcmsBL.All
                                            where e.SiglaDestino.Equals(entity.Icms.EstadoDestino, StringComparison.InvariantCultureIgnoreCase)
                                            && e.SiglaOrigem.Equals(entity.Icms.EstadoOrigem, StringComparison.InvariantCultureIgnoreCase)
                                            select e.IcmsAliquota).FirstOrDefault();

            double difalAliquota = aliquotaIntraestadual - aliquotaInterestadual;
            double baseIcms = entity.ValorBase;

            return new DifalRetornoVM { AliquotaIntraestadual = aliquotaIntraestadual, AliquotaInterestadual = aliquotaInterestadual, DiferencialDeAliquota = difalAliquota, ValorDifalOrigem = Math.Round(baseIcms / 100 * difalAliquota * 0.2,2), ValorDifalDestino = Math.Round(baseIcms / 100 * difalAliquota * 0.8,2), AgregaTotalNota = false };
        }
    }
}
