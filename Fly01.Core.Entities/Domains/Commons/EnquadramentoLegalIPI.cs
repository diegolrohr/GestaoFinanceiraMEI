using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class EnquadramentoLegalIPI : DomainBase
    {
        public string Codigo { get; set; }

        public string GrupoCST { get; set; }

        [MaxLength(600)]
        public string Descricao { get; set; }
    }
}