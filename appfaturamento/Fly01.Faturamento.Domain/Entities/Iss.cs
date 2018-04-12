using Fly01.Core.Entities.Domains;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Faturamento.Domain.Entities
{
    public class Iss : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        [Required]
        public string Descricao { get; set; }
    }
}
