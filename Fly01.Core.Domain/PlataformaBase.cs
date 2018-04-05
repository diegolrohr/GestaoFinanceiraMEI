using System.ComponentModel.DataAnnotations;
using Fly01.Core.Notifications;

namespace Fly01.Core.Domain
{
    public class PlataformaBase : DomainBase
    {
        [Required]
        public string PlataformaId { get; set; }
    }
}