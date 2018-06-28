using Fly01.Core.Entities.Domains;
using System.ComponentModel.DataAnnotations;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.IBPT
{
    public class IbptNcm : DomainBase
    {
        [Required]
        public string Codigo { get; set; }

        public double ImpostoNacional { get; set; }

        public double ImpostoEstadual { get; set; }

        public double ImpostoMunicipal { get; set; }

        public double ImpostoImportacao { get; set; }

        public string Versao { get; set; }

        public string UF { get; set; }
    }
}
