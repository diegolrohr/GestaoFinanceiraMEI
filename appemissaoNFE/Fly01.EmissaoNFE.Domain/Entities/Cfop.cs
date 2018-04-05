using Fly01.EmissaoNFE.Domain.Enums;
using Fly01.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Fly01.EmissaoNFE.Domain
{
    public class Cfop : DomainBase
    {
        [Required]
        public int Codigo { get; set; }

        [Required]
        [StringLength(400)]
        public string Descricao { get; set; }

        [Required]
        public TipoCFOP Tipo { get; set; }
    }
}
