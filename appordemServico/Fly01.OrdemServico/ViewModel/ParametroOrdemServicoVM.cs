using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.OrdemServico.ViewModel
{
    public class ParametroOrdemServicoVM : DomainBaseVM
    {
        [JsonProperty("diasPrazoEntrega")]
        public int DiasPrazoEntrega { get; set; }


    }
}
