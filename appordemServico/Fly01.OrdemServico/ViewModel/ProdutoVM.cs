using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class ProdutoVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [APIEnum("TipoProduto")]
        [JsonProperty("tipoProduto")]
        public string TipoProduto { get; set; }

        [JsonProperty("saldoProduto")]
        public double? SaldoProduto { get; set; }

        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("unidadeMedidaId")]
        public Guid? UnidadeMedidaId { get; set; }

        [JsonProperty("valorVenda")]
        public double ValorVenda { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [APIEnum("ObjetoDeManutencao")]
        [JsonProperty("objetoDeManutencao")]
        public string ObjetoDeManutencao { get; set; }

        [JsonProperty("unidadeMedida")]
        public virtual UnidadeMedidaVM UnidadeMedida { get; set; }

        [JsonProperty("registroFixo")]
        public bool RegistroFixo { get; set; }
    }
}