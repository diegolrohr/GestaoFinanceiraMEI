using System;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class OrdemVendaVM : DomainBaseVM
    {
        [JsonProperty("numero")]
        public int Numero { get; set; }

        [JsonProperty("numeracaoVolumesTrans")]
        public string NumeracaoVolumesTrans { get; set; }

        [JsonProperty("marca")]
        public string Marca{ get; set; }

        [JsonProperty("chaveNFeReferenciada")]
        public string ChaveNFeReferenciada { get; set; }

        [JsonProperty("tipoOrdemVenda")]
        [APIEnum("TipoOrdemVenda")]
        public string TipoOrdemVenda { get; set; }

        [JsonProperty("tipoVenda")]
        [APIEnum("TipoCompraVenda")]
        public string TipoVenda { get; set; }

        [JsonProperty("status")]
        [APIEnum("Status")]
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

        [JsonProperty("tipoEspecie")]
        public string TipoEspecie { get; set; }

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

        [JsonProperty("mensagemPadraoNota")]
        public string MensagemPadraoNota { get; set; }

        [JsonProperty("informacoesCompletamentaresNFS")]
        public string InformacoesCompletamentaresNFS { get; set; }

        [JsonProperty("tipoNfeComplementar")]
        [APIEnum("TipoNfeComplementar")]
        public string TipoNfeComplementar { get; set; }

        [JsonProperty("nFeRefComplementarIsDevolucao")]
        public bool NFeRefComplementarIsDevolucao { get; set; }

        [JsonProperty("centroCustoId")]
        public Guid? CentroCustoId { get; set; }

        [JsonProperty("ufSaidaPaisId")]
        public Guid? UFSaidaPaisId { get; set; }

        [JsonProperty("localEmbarque")]
        public string LocalEmbarque { get; set; }

        [JsonProperty("localDespacho")]
        public string LocalDespacho { get; set; }

        [JsonProperty("total")]        public double Total { get; set; }        [JsonProperty("totalRetencoesServicos")]        public double? TotalRetencoesServicos { get; set; }

        [JsonProperty("totalImpostosProdutos")]
        public double? TotalImpostosProdutos { get; set; }

        [JsonProperty("totalImpostosProdutosNaoAgrega")]
        public double TotalImpostosProdutosNaoAgrega { get; set; }

        [JsonProperty("totalImpostosServicosNaoAgrega")]
        public double TotalImpostosServicosNaoAgrega { get; set; }
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

        [JsonProperty("centroCusto")]
        public virtual CentroCustoVM CentroCusto { get; set; }

        [JsonProperty("ufSaidaPais")]
        public virtual EstadoVM UFSaidaPais { get; set; }
    }
}   