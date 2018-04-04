using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Fly01.Core.VM;
using Fly01.Core.Api;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class GrupoTributarioVM : DomainBaseVM
    {
        [JsonProperty("descricao")]
        public string Descricao { get; set; }

        [JsonProperty("cfopId")]
        public Guid CfopId { get; set; }

        //ICMS
        [Required]
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
        [Required]
        [JsonProperty("calculaIpi")]
        public bool CalculaIpi { get; set; }

        [Display(Name = "Situação Tributária IPI")]
        [JsonProperty("tipoTributacaoIPI")]
        [APIEnum("TipoTributacaoIPI")]
        public string TipoTributacaoIPI { get; set; }

        [JsonProperty("aplicaFreteBaseIpi")]
        public bool AplicaFreteBaseIpi { get; set; }

        [JsonProperty("aplicaDespesaBaseIpi")]
        public bool AplicaDespesaBaseIpi { get; set; }


        //PIS
        [Required]
        [JsonProperty("calculaPis")]
        public bool CalculaPis { get; set; }

        [Display(Name = "Situação Tributária PIS")]
        [JsonProperty("tipoTributacaoPIS")]
        [APIEnum("TipoTributacaoPISCOFINS")]
        public string TipoTributacaoPIS { get; set; }

        [JsonProperty("aplicaFreteBasePis")]
        public bool AplicaFreteBasePis { get; set; }

        [JsonProperty("aplicaDespesaBasePis")]
        public bool AplicaDespesaBasePis { get; set; }


        //COFINS
        [Required]
        [JsonProperty("calculaCofins")]
        public bool CalculaCofins { get; set; }

        [Display(Name = "Situação Tributária COFINS")]
        [JsonProperty("tipoTributacaoCOFINS")]
        [APIEnum("TipoTributacaoPISCOFINS")]
        public string TipoTributacaoCOFINS { get; set; }

        [JsonProperty("aplicaFreteBaseCofins")]
        public bool AplicaFreteBaseCofins { get; set; }

        [JsonProperty("aplicaDespesaBaseCofins")]
        public bool AplicaDespesaBaseCofins { get; set; }


        //ISS
        [Required]
        [JsonProperty("calculaIss")]
        public bool CalculaIss { get; set; }

        [Display(Name = "Situação Tributária ISS")]
        [JsonProperty("tipoTributacaoISS")]
        [APIEnum("TipoTributacaoISS")]
        public string TipoTributacaoISS { get; set; }

        [Display(Name = "Pagamento de Imposto ISS")]
        [JsonProperty("tipoPagamentoImpostoISS")]
        [APIEnum("TipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISS { get; set; }

        [Display(Name = "Tipo CFPS")]
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
        public virtual CfopVM Cfop { get; set; }

        #endregion
    }
}