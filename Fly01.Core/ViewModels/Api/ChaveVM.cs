using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.ViewModels.Api

{
    public class ChaveVM : EntidadeVM
    {
        [Required]
        public int CodigoUF { get; set; }

        [Required]
        public DateTime Emissao { get; set; }

        [Required]
        public int Modelo { get; set; }

        [Required]
        public int Serie { get; set; }

        [Required]
        public TipoNota TipoDocumentoFiscal { get; set; }

        [Required]
        public int NumeroNF { get; set; }

        [Required]
        public int CodigoNF { get; set; }

        [Required]
        public string Cnpj { get; set; }
    }
}
