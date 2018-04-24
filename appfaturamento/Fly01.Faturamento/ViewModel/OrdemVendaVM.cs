﻿using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class OrdemVendaVM : DomainBaseVM
    {
        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("tipoOrdemVenda")]
        [APIEnum("TipoOrdemVenda")]
        public string TipoOrdemVenda { get; set; }

        [JsonProperty("tipoVenda")]
        [APIEnum("TipoVenda")]
        public string TipoVenda { get; set; }

        [JsonProperty("status")]
        [APIEnum("StatusOrdemVenda")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("clienteId")]
        public Guid ClienteId { get; set; }

        [JsonProperty("grupoTributarioPadraoId")]
        public Guid? GrupoTributarioPadraoId { get; set; }

        [JsonProperty("transportadoraId")]
        public Guid? TransportadoraId { get; set; }

        [JsonProperty("tipoFrete")]
        [APIEnum("TipoFrete")]
        public string TipoFrete { get; set; }

        [JsonProperty("placaVeiculo")]
        public string PlacaVeiculo { get; set; }

        [JsonProperty("estadoPlacaVeiculoId")]
        public Guid? EstadoPlacaVeiculoId { get; set; }

        [JsonProperty("valorFrete")]
        public double? ValorFrete { get; set; }

        [JsonProperty("pesoBruto")]
        public double? PesoBruto { get; set; }

        [JsonProperty("pesoLiquido")]
        public double? PesoLiquido { get; set; }

        [JsonProperty("quantidadeVolumes")]
        public int? QuantidadeVolumes { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid? FormaPagamentoId { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid? CondicaoParcelamentoId { get; set; }

        [JsonProperty("categoriaId")]
        public Guid? CategoriaId { get; set; }

        [JsonProperty("dataVencimento")]
        public DateTime? DataVencimento { get; set; }

        [JsonProperty("movimentaEstoque")]
        public bool MovimentaEstoque { get; set; }

        [JsonProperty("ajusteEstoqueAutomatico")]
        public bool AjusteEstoqueAutomatico { get; set; }        

        [JsonProperty("geraFinanceiro")]
        public bool GeraFinanceiro { get; set; }

        [JsonProperty("geraNotaFiscal")]
        public bool GeraNotaFiscal { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("naturezaOperacao")]
        public string NaturezaOperacao { get; set; }

        [JsonProperty("cliente")]
        public virtual PessoaVM Cliente { get; set; }

        [JsonProperty("grupoTributarioPadrao")]
        public virtual GrupoTributarioVM GrupoTributarioPadrao { get; set; }

        [JsonProperty("transportadora")]
        public virtual PessoaVM Transportadora { get; set; }

        [JsonProperty("estadoPlacaVeiculo")]
        public virtual EstadoVM EstadoPlacaVeiculo { get; set; }

        [JsonProperty("condicaoParcelamento")]
        public virtual CondicaoParcelamentoVM CondicaoParcelamento { get; set; }

        [JsonProperty("formaPagamento")]
        public virtual FormaPagamentoVM FormaPagamento { get; set; }

        [JsonProperty("categoria")]
        public virtual CategoriaVM Categoria { get; set; }
    }
}