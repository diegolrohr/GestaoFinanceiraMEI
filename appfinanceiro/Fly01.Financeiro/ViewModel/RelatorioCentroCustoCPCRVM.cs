using System;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Financeiro.ViewModel
{
    public class RelatorioCentroCustoCPCRVM : EmpresaBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("clienteId")]
        public Guid ClienteId { get; set; }

        [JsonProperty("formaPagamentoId")]
        public Guid? FormaPagamentoId { get; set; }

        [JsonProperty("dataEmissaoInicial")]
        public DateTime? DataEmissaoInicial { get; set; }

        [JsonProperty("dataEmissaoFinal")]
        public DateTime? DataEmissaoFinal { get; set; }

        [JsonProperty("dataInicial")]
        public DateTime? DataInicial { get; set; }

        [JsonProperty("dataFinal")]
        public DateTime? DataFinal { get; set; }

        [JsonProperty("condicaoParcelamentoId")]
        public Guid? CondicaoParcelamentoId { get; set; }

        [JsonProperty("categoriaFinanceiraId")]
        public Guid? CategoriaFinanceiraId { get; set; }

        [JsonProperty("valor")]
        public double? Valor { get; set; }

        [JsonProperty("Numero")]
        public int? Numero { get; set; }

    }
}