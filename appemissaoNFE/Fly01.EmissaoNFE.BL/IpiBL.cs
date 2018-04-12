using Fly01.EmissaoNFE.Domain;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System;
using System.Linq;
using Fly01.Core.BL;

namespace Fly01.EmissaoNFE.BL
{
    public class IpiBL
    {
        protected NcmBL NcmBL;

        public IpiBL(AppDataContextBase context, NcmBL ncmBL)
        {
            NcmBL = ncmBL;
        }
        
        public TributacaoRetornoBaseVM Ipi(Tributacao entity)
        {
            entity.Ipi.Base = entity.Ipi.DespesaNaBase ? entity.ValorBase + entity.ValorDespesa : entity.ValorBase;
            entity.Ipi.Base += entity.Ipi.FreteNaBase ? entity.ValorFrete : 0;

            if (entity.Ipi.AliquotaPeloNcm)
                entity.Ipi.Aliquota = (from x in NcmBL.All
                            where x.Codigo.Equals(entity.Ipi.Ncm, StringComparison.InvariantCultureIgnoreCase)
                            select x.AliquotaIPI).FirstOrDefault();
            
            entity.Ipi.Valor = Math.Round(entity.Ipi.Base / 100 * entity.Ipi.Aliquota,2);

            return new TributacaoRetornoBaseVM { Base = entity.Ipi.Base, Aliquota = entity.Ipi.Aliquota, Valor = entity.Ipi.Valor, AgregaTotalNota = true };
        }
    }
}
