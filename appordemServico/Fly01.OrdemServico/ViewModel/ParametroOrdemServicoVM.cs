using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using System;

namespace Fly01.OrdemServico.ViewModel
{
    public class ParametroOrdemServicoVM : DomainBaseVM
    {
        [JsonProperty("diasPrazoEntrega")]
        public int DiasPrazoEntrega { get; set; }

        [JsonProperty("responsavelPadraoId")]
        public Guid? ResponsavelPadraoId { get; set; }

        [JsonProperty("responsavelPadrao")]
        public PessoaVM ResponsavelPadrao { get; set; }
    }
}
