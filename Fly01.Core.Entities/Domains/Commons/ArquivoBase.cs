using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class ArquivoBase : PlataformaBase
    {
        [Required]
        [MaxLength(50)]
        public string Descricao { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Conteudo { get; set; }

        [Required]
        [MaxLength(200)]
        public string Md5 { get; set; }

        [Required]
        [MaxLength(30)]
        public string Cadastro { get; set; }
        public double TotalProcessado { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        [MaxLength(int.MaxValue)]
        public string Retorno { get; set; }
    }
}
