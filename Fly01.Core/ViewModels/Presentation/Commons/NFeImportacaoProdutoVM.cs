using Fly01.Core.Entities.Domains.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class NFeImportacaoProdutoVM : DomainBaseVM
    {
        [JsonProperty("nfeImportacaoId")]
        public Guid NFeImportacaoId { get; set; }

        [JsonProperty("novoProduto")]
        public bool NovoProduto { get; set; }

        [JsonProperty("produtoId")]
        public Guid? ProdutoId { get; set; }

        [JsonProperty("codigo")]
        public string Codigo { get; set; }

        [JsonProperty("codigoBarras")]
        public string CodigoBarras { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid UnidadeMedidaId { get; set; }

        [JsonProperty("fatorConversao")]
        public double FatorConversao { get; set; }

        [JsonProperty("tipoFatorConversao")]
        public string TipoFatorConversao { get; set; }

        [JsonProperty("movimentaEstoque")]
        public bool MovimentaEstoque { get; set; }

        [JsonProperty("atualizaDadosProduto")]
        public bool AtualizaDadosProduto { get; set; }

        [JsonProperty("atualizaValorVenda")]
        public bool AtualizaValorVenda { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("pedidoItemId")]
        public Guid? PedidoItemId { get; set; }

        [JsonProperty("nfeImportacao")]
        public virtual NFeImportacao NFeImportacao { get; set; }
        [JsonProperty("produto")]
        public virtual Produto Produto { get; set; }
        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedida UnidadeMedida { get; set; }
        [JsonProperty("pedidoItem")]
        public virtual PedidoItem PedidoItem { get; set; }

    }
}