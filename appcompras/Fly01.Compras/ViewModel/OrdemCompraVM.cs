using System;
using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public class OrdemCompraVM : DomainBaseVM
    {
        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("chaveNFeReferenciada")]
        public string ChaveNFeReferenciada { get; set; }

        [JsonProperty("status")]
        [APIEnum("StatusOrdemCompra")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("grupoTributarioPadraoId")]
        public Guid? GrupoTributarioPadraoId { get; set; }

        [JsonProperty("placaVeiculo")]
        public string PlacaVeiculo { get; set; }

        [JsonProperty("estadoPlacaVeiculoId")]
        public Guid? EstadoPlacaVeiculoId { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid? FormaPagamentoId { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid? CondicaoParcelamentoId { get; set; }

        [JsonProperty("categoriaId")]
        public Guid? CategoriaId { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime? DataVencimento { get; set; }

        [JsonProperty("ajusteEstoqueAutomatico")]
        public bool AjusteEstoqueAutomatico { get; set; }

        [JsonProperty("naturezaOperacao")]
        public string NaturezaOperacao { get; set; }

        [JsonProperty("mensagemPadraoNota")]
        public string MensagemPadraoNota { get; set; }

        [JsonProperty("tipoOrdemCompra")]
        [APIEnum("TipoOrdemCompra")]
        public string TipoOrdemCompra { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("totalImpostosProdutos")]
        public double? TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double TotalImpostosProdutosNaoAgrega { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("centroCustoId")]
        public Guid? CentroCustoId { get; set; }

        [JsonProperty("estadoPlacaVeiculo")]
        public virtual EstadoVM EstadoPlacaVeiculo { get; set; }

        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }

        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }

        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }

        [JsonProperty("grupoTributarioPadrao")]
        public virtual GrupoTributarioVM GrupoTributarioPadrao { get; set; }

        [JsonProperty("centroCusto")]
        public virtual CentroCustoVM CentroCusto { get; set; }
    }
}