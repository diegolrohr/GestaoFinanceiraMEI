using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using System;

namespace Fly01.EmissaoNFE.Domain
{
    public class TabelaIcms : DomainBase
    {
        public string SiglaOrigem { get; set; }
        public string SiglaDestino { get; set; }
        public double IcmsAliquota { get; set; }
    }
}
