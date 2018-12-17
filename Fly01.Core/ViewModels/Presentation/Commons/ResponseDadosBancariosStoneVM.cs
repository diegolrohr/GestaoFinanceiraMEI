using Newtonsoft.Json;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class ResponseDadosBancariosStoneVM
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("bancoNome")]
        public string BancoNome { get; set; }

        [JsonProperty("bancoCodigo")]
        public int BancoCodigo { get; set; }

        [JsonProperty("contaTipo")]
        public string ContaTipo { get; set; }

        [JsonProperty("agencia")]
        public string Agencia { get; set; }

        [JsonProperty("agenciaDigito")]
        public string AgenciaDigito { get; set; }

        [JsonProperty("agenciaComDigito")]
        public string AgenciaComDigito
        {
            get
            {
                return Agencia + " - " + AgenciaDigito;
            }
            set { }
        }

        [JsonProperty("contaNumero")]
        public string ContaNumero { get; set; }

        [JsonProperty("contaDigito")]
        public string ContaDigito { get; set; }

        [JsonProperty("contaComDigito")]
        public string ContaComDigito
        {
            get
            {
                return ContaNumero + " - " + ContaDigito;
            }
            set { }
        }
    }
}