using Fly01.Core.VM;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class ProdutosMenosMovimentadosVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("saldoProduto")]
        public double? SaldoProduto { get; set; }
        [JsonProperty("totalMovimentos")]
        public int TotalMovimentos { get; set; }
    }
}