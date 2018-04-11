using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Domain
{
    public class PlataformaBase : DomainBase
    {
        [Required]
        public string PlataformaId { get; set; }
    }
}