using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class TemplateBoleto : PlataformaBase
    {
        public string Assunto { get; set; }

        [StringLength (5000)]
        public string Mensagem { get; set; }
    }
}
