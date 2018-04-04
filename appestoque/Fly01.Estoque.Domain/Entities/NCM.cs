using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Fly01.Core.Api.Domain;

namespace Fly01.Estoque.Domain.Entities
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