using Fly01.Core.Entities.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class GrupoTributarioBaseVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("cfopId")]
        public Guid CfopId { get; set; }

        //ICMS
        [JsonProperty("calculaIcms")]
        public bool CalculaIcms { get; set; }

        [JsonProperty("tipoTributacaoICMS")]
        [APIEnum("TipoTributacaoICMS")]
        public string TipoTributacaoICMS { get; set; }

        [JsonProperty("calculaIcmsDifal")]
        public bool CalculaIcmsDifal { get; set; }

        [JsonProperty("aplicaIpiBaseIcms")]
        public bool AplicaIpiBaseIcms { get; set; }

        [JsonProperty("aplicaFreteBaseIcms")]
        public bool AplicaFreteBaseIcms { get; set; }

        [JsonProperty("aplicaDespesaBaseIcms")]
        public bool AplicaDespesaBaseIcms { get; set; }


        //IPI
        [JsonProperty("calculaIpi")]
        public bool CalculaIpi { get; set; }

        [JsonProperty("tipoTributacaoIPI")]
        [APIEnum("TipoTributacaoIPI")]
        public string TipoTributacaoIPI { get; set; }

        [JsonProperty("aplicaFreteBaseIpi")]
        public bool AplicaFreteBaseIpi { get; set; }

        [JsonProperty("aplicaDespesaBaseIpi")]
        public bool AplicaDespesaBaseIpi { get; set; }


        //PIS
        [JsonProperty("calculaPis")]
        public bool CalculaPis { get; set; }

        [JsonProperty("tipoTributacaoPIS")]
        [APIEnum("TipoTributacaoPISCOFINS")]
        public string TipoTributacaoPIS { get; set; }

        [JsonProperty("aplicaFreteBasePis")]
        public bool AplicaFreteBasePis { get; set; }

        [JsonProperty("aplicaDespesaBasePis")]
        public bool AplicaDespesaBasePis { get; set; }


        //COFINS
        [JsonProperty("calculaCofins")]
        public bool CalculaCofins { get; set; }

        [JsonProperty("tipoTributacaoCOFINS")]
        [APIEnum("TipoTributacaoPISCOFINS")]
        public string TipoTributacaoCOFINS { get; set; }

        [JsonProperty("aplicaFreteBaseCofins")]
        public bool AplicaFreteBaseCofins { get; set; }

        [JsonProperty("aplicaDespesaBaseCofins")]
        public bool AplicaDespesaBaseCofins { get; set; }


        //ISS
        [JsonProperty("calculaIss")]
        public bool CalculaIss { get; set; }

        [JsonProperty("tipoTributacaoISS")]
        [APIEnum("TipoTributacaoISS")]
        public string TipoTributacaoISS { get; set; }

        [JsonProperty("tipoPagamentoImpostoISS")]
        [APIEnum("TipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISS { get; set; }

        [JsonProperty("tipoCFPS")]
        [APIEnum("TipoCFPS")]
        public string TipoCFPS { get; set; }


        //ST
        [JsonProperty("calculaSubstituicaoTributaria")]
        public bool CalculaSubstituicaoTributaria { get; set; }

        [JsonProperty("aplicaFreteBaseST")]
        public bool AplicaFreteBaseST { get; set; }

        [JsonProperty("aplicaDespesaBaseST")]
        public bool AplicaDespesaBaseST { get; set; }

        [JsonProperty("aplicaIpiBaseST")]
        public bool AplicaIpiBaseST { get; set; }

        #region Navigations Properties
        [JsonProperty("cfop")]
        public virtual CfopBaseVM Cfop { get; set; }
        #endregion
    }
}
