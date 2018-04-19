using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class CondicaoParcelamentoParcela
    {
        public string DescricaoParcela { get; set; }

        [Column(TypeName = "Date")]
        public DateTime DataVencimento { get; set; }

        public double Valor { get; set; }
    }
}
