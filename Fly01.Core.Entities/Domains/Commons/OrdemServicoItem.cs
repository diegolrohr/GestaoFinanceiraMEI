using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class OrdemServicoItem : OrdemServicoItemBase
    {
        public double Valor { get; set; }

        public double Desconto { get; set; }

        [NotMapped]
        public double Total => Quantidade > 0 ? Math.Round(((Quantidade * Valor) - Desconto), 2, MidpointRounding.AwayFromZero) : Math.Round((Valor - Desconto), 2, MidpointRounding.AwayFromZero);
    }
}
