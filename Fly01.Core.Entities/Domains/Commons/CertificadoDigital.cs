using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class CertificadoDigital : PlataformaBase
    {
        public int Tipo { get; set; }

        public DateTime? DataEmissao { get; set; }

        public DateTime? DataExpiracao { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Certificado { get; set; }

        [Required]
        public string Senha { get; set; }

        [MaxLength(6)]
        public string EntidadeHomologacao { get; set; }

        [MaxLength(6)]
        public string EntidadeProducao { get; set; }

        [MaxLength(30)]
        public string Versao { get; set; }

        public string Emissor { get; set; }

        public string Pessoa { get; set; }

        [Required]
        [MaxLength(32)]
        public string Md5 { get; set; }

        [MaxLength(16)]
        public string Cnpj { get; set; }

        [MaxLength(18)]
        public string InscricaoEstadual { get; set; }

        [MaxLength(2)]
        public string UF { get; set; }
    }
}