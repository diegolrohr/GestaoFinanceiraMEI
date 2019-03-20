using Fly01.Core.Helpers.Attribute;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.ViewModel
{
    [Serializable]
    public class PedidoVM : OrdemCompraVM
    {
        [JsonProperty("fornecedorId")]
        public Guid FornecedorId { get; set; }

        [JsonProperty("transportadoraId")]
        public Guid? TransportadoraId { get; set; }

        [JsonProperty("numeracaoVolumesTrans")]
        public string NumeracaoVolumesTrans { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; }

        [JsonProperty("tipoEspecie")]
        public string TipoEspecie { get; set; }

        [JsonProperty("tipoCompra")]
        [APIEnum("TipoCompraVenda")]
        public string TipoCompra { get; set; }

        [JsonProperty("tipoFrete")]
        [APIEnum("TipoFrete")]
        public string TipoFrete { get; set; }

        [JsonProperty("valorFrete")]
        public double? ValorFrete { get; set; }

        [JsonProperty("pesoBruto")]
        public double? PesoBruto { get; set; }

        [JsonProperty("pesoLiquido")]
        public double? PesoLiquido { get; set; }

        [JsonProperty("quantidadeVolumes")]
        public int? QuantidadeVolumes { get; set; }

        [JsonProperty("movimentaEstoque")]
        public bool MovimentaEstoque { get; set; }

        [JsonProperty("geraFinanceiro")]
        public bool GeraFinanceiro { get; set; }

        [JsonProperty("geraNotaFiscal")]
        public bool GeraNotaFiscal { get; set; }

        [JsonProperty("orcamentoOrigemId")]
        public Guid? OrcamentoOrigemId { get; set; }

        [JsonProperty("fornecedor")]
        public virtual PessoaVM Fornecedor { get; set; }

        [JsonProperty("transportadora")]
        public virtual PessoaVM Transportadora { get; set; }

        [JsonProperty("orcamentoOrigem")]
        public virtual OrcamentoVM OrcamentoOrigem { get; set; }
    }
}