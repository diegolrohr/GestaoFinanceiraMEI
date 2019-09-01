using Newtonsoft.Json;
using System;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class ImprimirListContasVM : ImprimirListContasFiltroVM
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("valor")]
        public double Valor { get; set; }
        [JsonProperty("formaPagamento")]
        public string FormaPagamento { get; set; }
        [JsonProperty("numero")]
        public int? Numero { get; set; }
        [JsonProperty("fornecedor")]
        public string Fornecedor { get; set; }
        [JsonProperty("cliente")]
        public string Cliente { get; set; }
        [JsonProperty("vencimento")]
        public DateTime? Vencimento { get; set; }
        [JsonProperty("emissao")]
        public DateTime? Emissao { get; set; }
        [JsonProperty("titulo")]
        public string Titulo { get; set; }
        [JsonProperty("categoria")]
        public string Categoria { get; set; }
        [JsonProperty("conicaoParcelamento")]
        public string CondicaoParcelamento { get; set; }
        [JsonProperty("parcela")]
        public string Parcela { get; set; }
        [JsonProperty("tipoConta")]
        public string TipoConta { get; set; }
        public string Filtro { get; set; }
    }
}