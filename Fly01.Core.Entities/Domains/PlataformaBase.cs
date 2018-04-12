using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains
{
    public class PlataformaBase : DomainBase
    {
        [Required]
        public string PlataformaId { get; set; }
    }
}