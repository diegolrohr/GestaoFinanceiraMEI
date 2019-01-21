using System;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class StoneAntecipacaoRecebiveis : PlataformaBase
    {
        public double ValorBruto { get; set; }

        public double ValorAntecipado { get; set; }

        public double TaxaPontual { get; set; }

        public DateTime Data { get; set; }

        public int StoneId { get; set; }

        public int StoneBancoId { get; set; }
    }
}
