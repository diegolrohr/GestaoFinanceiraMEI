using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fly01.Core.Domain;

namespace Fly01.Financeiro.Domain.Entities
{
    public class Arquivo : PlataformaBase
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

        //#region Notifications

        //[NotMapped]
        //public Notification Notification { get; } = new Notification();

        //#endregion
    }
}