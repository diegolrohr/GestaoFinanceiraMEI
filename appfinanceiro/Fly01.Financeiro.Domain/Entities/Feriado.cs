using Fly01.Core.Api.Domain;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Financeiro.Domain.Entities
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
