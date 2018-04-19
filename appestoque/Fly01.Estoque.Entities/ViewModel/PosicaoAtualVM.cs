using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Fly01.Estoque.Entities.ViewModel
{
    [Serializable]
    public class PosicaoAtualVM : DomainBaseVM
    {
        [JsonProperty("produtos")]
        public List<ProdutoVM> Produtos { get; set; }

        [JsonProperty("estoqueTotal")]
        public double? EstoqueTotal { get; set; }

        [JsonProperty("custoTotal")]
        public double CustoTotal { get; set; }

        [JsonProperty("vendaTotal")]
        public double VendaTotal { get; set; }
    }
}
