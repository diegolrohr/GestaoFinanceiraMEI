using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Faturamento.ViewModel
{
    [Serializable]
    public class NotaFiscalVM : DomainBaseVM
    {

        [JsonProperty("numeracaoVolumesTrans")]
        public int NumeracaoVolumesTrans { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; }

        [JsonProperty("ordemVendaOrigemId")]
        public Guid OrdemVendaOrigemId { get; set; }

        [JsonProperty("TipoNotaFiscal")]
        [APIEnum("TipoNotaFiscal")]
        public string TipoNotaFiscal { get; set; }

        [JsonProperty("tipoVenda")]
        [APIEnum("TipoVenda")]
        public string TipoVenda { get; set; }

        [JsonProperty("status")]
        [APIEnum("StatusNotaFiscal")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public DateTime Data { get; set; }

        [JsonProperty("clienteId")]
        public Guid ClienteId { get; set; }

        [JsonProperty("transportadoraId")]
        public Guid? TransportadoraId { get; set; }

        [JsonProperty("tipoFrete")]
        [APIEnum("TipoFrete")]
        public string TipoFrete { get; set; }

        [JsonProperty("tipoEspecie")]
        [APIEnum("TipoEspecie")]
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

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("serieNotaFiscalId")]
        public Guid? SerieNotaFiscalId { get; set; }

        [JsonProperty("numNotaFiscal")]
        public int? NumNotaFiscal { get; set; }

        [JsonProperty("xml")]
        public string XML { get; set; }

        [JsonProperty("pdf")]
        public string PDF { get; set; }

        [JsonProperty("mensagem")]
        public string Mensagem { get; set; }

        [JsonProperty("recomendacao")]
        public string Recomendacao { get; set; }    

        [JsonProperty("naturezaOperacao")]        
        public string NaturezaOperacao { get; set; }

        [JsonProperty("mensagemPadraoNota")]
        public string MensagemPadraoNota { get; set; }

        [JsonProperty("sefazId")]
        public string SefazId { get; set; }

        [JsonProperty("ordemVendaOrigem")]
        public virtual OrdemVendaVM OrdemVendaOrigem { get; set; }

        [JsonProperty("cliente")]
        public virtual PessoaVM Cliente { get; set; }

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

        [JsonProperty("serieNotaFiscal")]
        public virtual SerieNotaFiscalVM SerieNotaFiscal { get; set; }
    }
}