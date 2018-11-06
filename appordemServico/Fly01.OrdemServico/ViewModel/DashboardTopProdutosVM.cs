using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.OrdemServico.ViewModel
{
    public class DashboardTopProdutosVM : DomainBaseVM
    {
        [JsonProperty("codigoProduto")]
        public string CodigoProduto { get; set; }

        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("quantidade")]
        public double Quantidade { get; set; }


    }
}
