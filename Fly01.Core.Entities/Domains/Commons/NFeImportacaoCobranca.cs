using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeImportacaoCobranca : PlataformaBase
    {
        [Required]
        public Guid NFeImportacaoId { get; set; }

        [StringLength(60)]
        public string Numero { get; set; }

        public double Valor { get; set; }

        public DateTime DataVencimento { get; set; }

        public Guid? ContaFinanceiraId { get; set; }

        public virtual NFeImportacao NFeImportacao { get; set; }
    }
}