using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Faturamento.Domain.Entities
{
    public class SubstituicaoTributaria : SubstituicaoTributariaBase
    {
        public virtual NCM Ncm { get; set; }
        public virtual Estado EstadoOrigem { get; set; }
        public virtual Estado EstadoDestino { get; set; }
        public virtual Cest Cest { get; set; }
    }
}