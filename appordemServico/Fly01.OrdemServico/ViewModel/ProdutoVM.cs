using Newtonsoft.Json;

namespace Fly01.OrdemServico.ViewModel
{
    public class ProdutoVM : Core.ViewModels.Presentation.Commons.ProdutoVM
    {
        [JsonProperty("objetoDeManutencao")]
        public bool ObjetoDeManutencao { get; set; }
    }
}