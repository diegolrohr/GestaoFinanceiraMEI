using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class ProdutosMaisMovimentadosVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("saldoProduto")]
        public double? SaldoProduto { get; set; }
        [JsonProperty("totalMovimentos")]
        public int TotalMovimentos { get; set; }
    }
}