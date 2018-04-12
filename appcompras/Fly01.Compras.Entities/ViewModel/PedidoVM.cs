using Fly01.Core.Entities.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class PedidoVM : OrdemCompraVM
    {
        [JsonProperty("fornecedorId")]
        public Guid FornecedorId { get; set; }

        [JsonProperty("transportadoraId")]
        public Guid? TransportadoraId { get; set; }

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

        [JsonProperty("orcamentoOrigemId")]
        public Guid? OrcamentoOrigemId { get; set; }

        #region Navigations Properties
        [JsonProperty("fornecedor")]
        public virtual PessoaVM Fornecedor { get; set; }
        [JsonProperty("transportadora")]
        public virtual PessoaVM Transportadora { get; set; }
        [JsonProperty("orcamentoOrigem")]
        public virtual OrcamentoVM OrcamentoOrigem { get; set; }

        #endregion
    }
}
