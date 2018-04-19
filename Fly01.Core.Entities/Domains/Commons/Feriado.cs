using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class Feriado : PlataformaBase
    {
        [Required]
        public string Descricao { get; set; }

        [Required]
        public int Dia { get; set; }

        [Required]
        public int Mes { get; set; }

        public int Ano { get; set; }

        [Required]
        public bool Recorrente { get; set; }
    }
}