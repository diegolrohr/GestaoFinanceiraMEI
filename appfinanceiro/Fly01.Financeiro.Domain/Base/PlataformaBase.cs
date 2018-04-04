using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Domain.Base
{
    public class PlataformaBase : DomainBase
    {
        [Required]
        public string PlataformaId { get; set; }
    }
}
