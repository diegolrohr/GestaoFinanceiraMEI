using Fly01.Core.Entities.Domains.Enum;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Core.Entities.Domains.Commons
{
    public class GrupoTributario : PlataformaBase
    {
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Descricao { get; set; }

        public Guid? CfopId { get; set; }

        //ICMS
        [Required]
        public bool CalculaIcms { get; set; }

        [JsonIgnore]
        public TipoTributacaoICMS? TipoTributacaoICMS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoICMS")]
        public string TipoTributacaoICMSRest
        {
            get { return TipoTributacaoICMS.HasValue ? ((int)TipoTributacaoICMS).ToString() : ""; }
            set
            {
                TipoTributacaoICMS = (!string.IsNullOrEmpty(value)) ?
                  (TipoTributacaoICMS)System.Enum.Parse(typeof(TipoTributacaoICMS), value) :
                  Enum.TipoTributacaoICMS.TributadaComPermissaoDeCredito;
            }
        }

        public bool CalculaIcmsDifal { get; set; }

        public bool AplicaIpiBaseIcms { get; set; }

        public bool AplicaFreteBaseIcms { get; set; }

        public bool AplicaDespesaBaseIcms { get; set; }

        [Required]
        public bool CalculaIpi { get; set; }

        [JsonIgnore]
        public TipoTributacaoIPI? TipoTributacaoIPI { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoIPI")]
        public string TipoTributacaoIPIRest
        {
            get { return TipoTributacaoIPI.HasValue ? ((int)TipoTributacaoIPI).ToString() : ""; }
            set
            {
                TipoTributacaoIPI = (!string.IsNullOrEmpty(value)) ?
                  (TipoTributacaoIPI)System.Enum.Parse(typeof(TipoTributacaoIPI), value) :
                      Enum.TipoTributacaoIPI.EntradaComRecuperacaoDeCredito;
            }
        }

        public bool AplicaFreteBaseIpi { get; set; }

        public bool AplicaDespesaBaseIpi { get; set; }

        [Required]
        public bool CalculaPis { get; set; }

        public bool RetemPis { get; set; }

        [JsonIgnore]
        public TipoTributacaoPISCOFINS? TipoTributacaoPIS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoPIS")]
        public string TipoTributacaoPISRest
        {
            get { return TipoTributacaoPIS.HasValue ? ((int)TipoTributacaoPIS).ToString() : ""; }
            set
            {
                TipoTributacaoPIS = (!string.IsNullOrEmpty(value)) ?
                  (TipoTributacaoPISCOFINS)System.Enum.Parse(typeof(TipoTributacaoPISCOFINS), value)
                  : TipoTributacaoPISCOFINS.T01;
            }
        }

        public bool AplicaFreteBasePis { get; set; }

        public bool AplicaDespesaBasePis { get; set; }

        [Required]
        public bool CalculaCofins { get; set; }

        public bool RetemCofins { get; set; }

        [JsonIgnore]
        public TipoTributacaoPISCOFINS? TipoTributacaoCOFINS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoCOFINS")]
        public string TipoTributacaoCOFINSRest
        {
            get { return TipoTributacaoCOFINS.HasValue ? ((int)TipoTributacaoCOFINS).ToString() : ""; }
            set
            {
                TipoTributacaoCOFINS = (!string.IsNullOrEmpty(value)) ?
                  (TipoTributacaoPISCOFINS)System.Enum.Parse(typeof(TipoTributacaoPISCOFINS), value)
                  : TipoTributacaoPISCOFINS.T01;
            }
        }

        public bool AplicaFreteBaseCofins { get; set; }

        public bool AplicaDespesaBaseCofins { get; set; }

        [Required]
        public bool CalculaIss { get; set; }

        public bool RetemISS { get; set; }

        [JsonIgnore]
        public TipoTributacaoISS? TipoTributacaoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoTributacaoISS")]
        public string TipoTributacaoISSRest
        {
            get { return TipoTributacaoISS.HasValue ? ((int)TipoTributacaoISS).ToString() : ""; }
            set
            {
                TipoTributacaoISS = (!string.IsNullOrEmpty(value)) ?
                  (TipoTributacaoISS)System.Enum.Parse(typeof(TipoTributacaoISS), value)
                  : Enum.TipoTributacaoISS.T00;
            }
        }

        [JsonIgnore]
        public TipoPagamentoImpostoISS? TipoPagamentoImpostoISS { get; set; }

        [NotMapped]
        [JsonProperty("tipoPagamentoImpostoISS")]
        public string TipoPagamentoImpostoISSRest
        {
            get { return TipoPagamentoImpostoISS.HasValue ? ((int)TipoPagamentoImpostoISS).ToString() : ""; }
            set
            {
                TipoPagamentoImpostoISS = (!string.IsNullOrEmpty(value)) ?
                  (TipoPagamentoImpostoISS)System.Enum.Parse(typeof(TipoPagamentoImpostoISS), value)
                  : Enum.TipoPagamentoImpostoISS.DentroMunicipio;
            }
        }

        [JsonIgnore]
        public TipoCFPS? TipoCFPS { get; set; }

        [NotMapped]
        [JsonProperty("tipoCFPS")]
        public string TipoCFPSRest
        {
            get { return TipoCFPS.HasValue ? ((int)TipoCFPS).ToString() : ""; }
            set
            {
                TipoCFPS = (!string.IsNullOrEmpty(value)) ?
                  (TipoCFPS)System.Enum.Parse(typeof(TipoCFPS), value)
                  : Enum.TipoCFPS.Tomador;
            }
        }

        public bool CalculaSubstituicaoTributaria { get; set; }

        public bool AplicaFreteBaseST { get; set; }

        public bool AplicaDespesaBaseST { get; set; }

        public bool AplicaIpiBaseST { get; set; }

        public bool CalculaCSLL { get; set; }

        public bool RetemCSLL { get; set; }

        public bool CalculaINSS { get; set; }

        public bool RetemINSS { get; set; }

        public bool CalculaImpostoRenda { get; set; }

        public bool RetemImpostoRenda { get; set; }

        public int CodigoCfop { get; set; }

        public virtual Cfop Cfop { get; set; }
    }
}