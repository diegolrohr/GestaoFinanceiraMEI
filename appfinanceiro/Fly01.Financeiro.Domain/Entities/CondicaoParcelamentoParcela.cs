using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Financeiro.Domain.Entities
{
    public class CondicaoParcelamentoParcela
    {
        public string DescricaoParcela { get; set; }

        [Column(TypeName = "Date")]
        public DateTime DataVencimento { get; set; }

        public double Valor { get; set; }
    }
}