using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class AliquotaSimplesNacionalVM : DomainBaseVM
    {
        [JsonProperty("tipoFaixaReceitaBruta")]
        public string TipoFaixaReceitaBruta { get; set; }

        [JsonProperty("tipoEnquadramentoEmpresa")]
        public string TipoEnquadramentoEmpresa { get; set; }

        [JsonProperty("simplesNacional")]
        public double SimplesNacional { get; set; }

        [JsonProperty("impostoRenda")]
        public double ImpostoRenda { get; set; }

        [JsonProperty("csll")]
        public double Csll { get; set; }

        [JsonProperty("cofins")]
        public double Cofins { get; set; }

        [JsonProperty("pisPasep")]
        public double PisPasep { get; set; }

        [JsonProperty("ipi")]
        public double Ipi { get; set; }

        [JsonProperty("iss")]
        public double Iss { get; set; }

        [JsonProperty("isOnCadastroParametros")]
        public bool IsOnCadastroParametros { get; set; }

        [JsonProperty("enviarEmailContador")]
        public bool EnviarEmailContador { get; set; }

        [JsonProperty("emailContador")]
        public string EmailContador { get; set; }
    }
}