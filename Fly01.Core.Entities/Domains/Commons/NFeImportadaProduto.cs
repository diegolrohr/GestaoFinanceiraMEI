using Fly01.Core.Entities.Domains.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class NFeImportacaoProduto : PlataformaBase
    {
        [Required]
        public Guid NFeImportadaId { get; set; }

        public Guid? ProdutoId { get; set; }

        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Quantidade { get; set; }

        public double Valor { get; set; }

        public Guid UnidadeMedidaId { get; set; }

        public double FatorConversao { get; set; }

        public TipoFatorConversao TipoFatorConversao { get; set; }

        public bool MovimentaEstoque { get; set; }

        public bool AtualizaDadosProduto { get; set; }

        public bool AtualizaValorCompra { get; set; }

        public bool AtualizaValorVenda { get; set; }

        public double ValorVenda { get; set; }

        public TipoAtualizacaoValor TipoValorVenda { get; set; }

        public virtual NFeImportacao NFeImportacao { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual UnidadeMedida UnidadeMedida { get; set; }
    }
}