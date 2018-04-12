using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;

namespace Fly01.Financeiro.Domain
{
    public class EstadoVM : DomainBaseVM
    {
        [Required]
        [StringLength(2)]
        public string Sigla { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(35)]
        public string UtcId { get; set; }

        [Required]
        [StringLength(2)]
        [Display(Name = "Código IBGE")]
        public string CodigoIBGE { get; set; }
    }
}