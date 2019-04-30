using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class AliquotaSimplesNacional : DomainBase
    {
        [Required]
        public TipoFaixaReceitaBruta TipoFaixaReceitaBruta { get; set; }

        [Required]
        public TipoEnquadramentoEmpresa TipoEnquadramentoEmpresa { get; set; }

        public double SimplesNacional { get; set; }

        public double ImpostoRenda { get; set; }

        public double Csll { get; set; }

        public double Cofins { get; set; }

        public double PisPasep { get; set; }

        public double Ipi { get; set; }

        public double Iss { get; set; }
    }
}
