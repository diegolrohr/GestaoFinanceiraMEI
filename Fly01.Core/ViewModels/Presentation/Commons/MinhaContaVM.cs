using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class MinhaContaVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("emissao")]
        public string DataEmissao { get; set; }

        [JsonProperty("nfe")]
        public string NFE { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("prefixo")]
        public string Prefixo{ get; set; }

        [JsonProperty("vencimento")]
        public string Vencimento { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        [JsonProperty("urlBoleto")]
        public string UrlBoleto { get; set; }

        [JsonProperty("urlNfe")]
        public string UrlNfe { get; set; }

        [JsonProperty("empresa")]
        public string Empresa { get; set; }

        [JsonProperty("filial")]
        public string Filial { get; set; }

        [JsonProperty("parcela")]
        public string Parcela { get; set; }
    }
}

