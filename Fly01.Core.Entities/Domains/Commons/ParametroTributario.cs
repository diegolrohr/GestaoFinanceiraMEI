using Newtonsoft.Json;
using Fly01.Core.Entities.Domains.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Fly01.Core.Entities.Domains.Commons
{
    public class ParametroTributario : PlataformaBase
    {
        public bool SimplesNacional { get; set; }

        public double AliquotaSimplesNacional { get; set; }

        public double AliquotaFCP { get; set; }

        public double AliquotaISS { get; set; }

        public double AliquotaPISPASEP { get; set; }

        public double AliquotaCOFINS { get; set; }

        public double AliquotaCSLL { get; set; }

        public double AliquotaINSS { get; set; }

        public double AliquotaImpostoRenda { get; set; }

        public bool RegistroSimplificadoMT { get; set; }

        [MaxLength(5000)]
        public string MensagemPadraoNota { get; set; }

        public string NumeroRetornoNF { get; set; }

        [JsonIgnore]
        public TipoAmbiente TipoAmbiente { get; set; }

        [NotMapped]
        [JsonProperty("tipoAmbiente")]
        public string TipoAmbienteRest
        {
            get { return ((int)TipoAmbiente).ToString(); }
            set { TipoAmbiente = (TipoAmbiente)System.Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoVersaoNFe TipoVersaoNFe { get; set; }

        [NotMapped]
        [JsonProperty("tipoVersaoNFe")]
        public string TipoVersaoNFeRest
        {
            get { return ((int)TipoVersaoNFe).ToString(); }
            set { TipoVersaoNFe = (TipoVersaoNFe)System.Enum.Parse(typeof(TipoVersaoNFe), value); }
        }

        [JsonIgnore]
        public TipoModalidade TipoModalidade { get; set; }

        [NotMapped]
        [JsonProperty("tipoModalidade")]
        public string TipoModalidadeRest
        {
            get { return ((int)TipoModalidade).ToString(); }
            set { TipoModalidade = (TipoModalidade)System.Enum.Parse(typeof(TipoModalidade), value); }
        }

        [MaxLength(16)]
        public string Cnpj { get; set; }

        [MaxLength(18)]
        public string InscricaoEstadual { get; set; }

        [MaxLength(2)]
        public string UF { get; set; }

        public TipoCRT TipoCRT { get; set; }

        public TipoPresencaComprador TipoPresencaComprador { get; set; }

        public HorarioVerao HorarioVerao { get; set; }

        public TipoHorarioTSS TipoHorario { get; set; }

        #region NFS
        public string VersaoNFSe { get; set; }

        [JsonIgnore]
        public TipoAmbiente TipoAmbienteNFS { get; set; }

        [NotMapped]
        [JsonProperty("tipoAmbienteNFS")]
        public string TipoAmbienteNFSRest
        {
            get { return ((int)TipoAmbienteNFS).ToString(); }
            set { TipoAmbienteNFS = (TipoAmbiente)System.Enum.Parse(typeof(TipoAmbiente), value); }
        }

        public bool IncentivoCultura { get; set; }

        public string UsuarioWebServer { get; set; }

        public string SenhaWebServer { get; set; }

        public string ChaveAutenticacao { get; set; }

        public string Autorizacao { get; set; }

        public bool FormatarCodigoISS { get; set; }

        /// <summary>
        /// NFS-e saiu depois de NF-e, então já tinha parâmetro salvo
        /// mas precisa reenviar para outra rota do TSS
        /// </summary>
        public bool ParametroValidoNFS { get; set; }

        public TipoTributacaoNFS TipoTributacaoNFS { get; set; }

        public TipoRegimeEspecialTributacao TipoRegimeEspecialTributacao { get; set; }

        #endregion
    }
}