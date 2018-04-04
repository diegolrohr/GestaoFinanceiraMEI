using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Financeiro.Domain.Entities
{
    [ComplexType]
    public class DadosEndereco
    {
        [Column("CEP")]
        public string CEP { get; set; }

        [Column("Endereco")]
        public string Endereco { get; set; }

        [Column("Bairro")]
        public string Bairro { get; set; }

        [Column("Cidade")]
        public string Cidade { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }
    }
}