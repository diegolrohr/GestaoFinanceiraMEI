using Fly01.Core.Helpers;
using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class MinhaContaVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("observacao")]
        public string Observacao { get; set; }

        [JsonProperty("emissao")]
        public DateTime Emissao { get; set; }

        [JsonProperty("vencimento")]
        public DateTime Vencimento { get; set; }

        [JsonProperty("nfe")]
        public string NFE { get; set; }

        [JsonProperty("numero")]
        public string Numero { get; set; }

        [JsonProperty("valor")]
        public double Valor { get; set; }

        [JsonProperty("prefixo")]
        public string Prefixo { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        //[JsonIgnore]
        //public TipoSituacao Situacao { get; set; }

        //[JsonProperty("situacao")]
        //public string SituacaoRest { get; set; }

        //[JsonProperty("situacao")]
        //public string SituacaoRest
        //{
        //    get
        //    {
        //        return EnumHelper.GetKey(typeof(TipoSituacao), Situacao.ToString());
        //    }
        //    set
        //    {
        //        Situacao = (TipoSituacao)Enum.Parse(typeof(TipoSituacao), value);
        //    }
        //}

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

    public enum TipoSituacao
    {
        [Subtitle("PENDENTE", "PENDENTE", "Pendente", "blue")]
        PENDENTE = 1,

        [Subtitle("PAGO", "PAGO", "Pago", "green")]
        PAGO = 2,
    }
}

