using Fly01.Core.Entities.Domains;
using Fly01.Financeiro.Domain.Enums;
using System;

namespace Fly01.Financeiro.Domain.Entities
{
    public class MovimentacaoPorCategoria : PlataformaBase
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