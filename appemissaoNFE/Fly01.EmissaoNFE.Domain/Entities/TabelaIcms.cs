using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using System;

namespace Fly01.EmissaoNFE.Domain
{
    public class TabelaIcms : DomainBase
    {
        public Guid EstadoOrigemId { get; set; }
        public Guid EstadoDestinoId { get; set; }
        public string SiglaOrigem { get; set; }
        public string SiglaDestino { get; set; }
        public double IcmsAliquota { get; set; }
                
        #region NavigationProperties

        public virtual Estado EstadoOrigem { get; set; }
        public virtual Estado EstadoDestino { get; set; }

        #endregion
    }
}
