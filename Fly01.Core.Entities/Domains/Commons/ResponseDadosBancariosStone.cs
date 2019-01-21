using Newtonsoft.Json;


namespace Fly01.Core.Entities.Domains.Commons
{
    public class ResponseDadosBancariosStone
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("bank")]
        public string BancoNome { get; set; }

        [JsonProperty("bank_id")]
        public int BancoCodigo { get; set; }

        [JsonProperty("account_type")]
        public string ContaTipo { get; set; }

        [JsonProperty("branch_code")]
        public string Agencia { get; set; }

        [JsonProperty("branch_digit")]
        public string AgenciaDigito { get; set; }

        [JsonProperty("account_number")]
        public string ContaNumero { get; set; }

        [JsonProperty("check_digit")]
        public string ContaDigito { get; set; }
    }
}
