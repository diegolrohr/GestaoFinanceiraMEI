using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [ComplexType]
    public class DadosContato
    {
        [Column("Contato")]
        public string Contato { get; set; }

        [Column("Email")]
        public string Email { get; set; }

        [Column("Telefone")]
        public string Telefone { get; set; }

        [Column("Celular")]
        public string Celular { get; set; }
    }
}