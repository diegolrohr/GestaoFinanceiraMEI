using System.ComponentModel.DataAnnotations;
using Fly01.Core.Domain;

namespace Fly01.Faturamento.Domain.Entities
{
    public class NCM : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Descricao { get; set; }

        public double AliquotaIPI { get; set; }
    }
}