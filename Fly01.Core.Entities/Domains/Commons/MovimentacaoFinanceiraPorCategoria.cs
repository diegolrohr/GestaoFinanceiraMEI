using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    [Table("MovimentacaoPorCategoria")]
    public class MovimentacaoFinanceiraPorCategoria : EmpresaBase
    {
        public Guid CategoriaId { get; set; }
        public string Categoria { get; set; }
        public Guid? CategoriaPaiId { get; set; }
        public double Previsto { get; set; }
        public double? Realizado { get; set; }
        public double? Soma { get; set; }
        public TipoCarteira TipoCarteira { get; set; }
        public TipoContaFinanceira TipoContaFinanceira { get; set; }
    }
}