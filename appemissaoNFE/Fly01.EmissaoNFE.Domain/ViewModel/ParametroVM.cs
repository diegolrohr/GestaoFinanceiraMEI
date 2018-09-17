using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using Newtonsoft.Json;
using System;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    public class ParametroVM : EntidadeVM
    {   
        public bool IncentivadorCultural { get; set; }

        public bool SimplesNacional { get; set; }

        #region NF-e

        public string VersaoDPEC { get; set; }

        public string NumeroRetornoNF { get; set; }

        public bool EnviaDanfe { get; set; }

        public bool UsaEPEC { get; set; }
        
        public string VersaoNFe { get; set; }
        
        [JsonIgnore]
        public TipoAmbiente Ambiente { get; set; }

        [JsonProperty("Ambiente")]
        public string TipoAmbiente
        {
            get { return ((int)Ambiente).ToString(); }
            set { Ambiente = (TipoAmbiente)Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoModalidade Modalidade { get; set; }
        
        [JsonProperty("Modalidade")]
        public string TipoModalidade
        {
            get { return ((int)Modalidade).ToString(); }
            set { Modalidade = (TipoModalidade)Enum.Parse(typeof(TipoModalidade), value); }
        }

        [JsonIgnore]
        public TipoRegimeTributario RegimeTributario { get; set; }
        
        [JsonProperty("RegimeTributario")]
        public string TipoRegimeTributario
        {
            get { return ((int)RegimeTributario).ToString(); }
            set { RegimeTributario = (TipoRegimeTributario)Enum.Parse(typeof(TipoRegimeTributario), value); }
        }

        [JsonIgnore]
        public HorarioVerao HorarioVerao { get; set; }

        [JsonProperty("HorarioVerao")]
        public string HorarioVeraoRest
        {
            get { return ((int)HorarioVerao).ToString(); }
            set { HorarioVerao = (HorarioVerao)Enum.Parse(typeof(HorarioVerao), value); }
        }

        [JsonIgnore]
        public TipoHorarioTSS TipoHorario { get; set; }

        [JsonProperty("TipoHorario")]
        public string TipoHorarioRest
        {
            get { return ((int)TipoHorario).ToString(); }
            set { TipoHorario = (TipoHorarioTSS)Enum.Parse(typeof(TipoHorarioTSS), value); }
        }

        #endregion

        #region NFS-e

        [JsonProperty("versaoNFSe")]
        public string VersaoNFSe { get; set; }

        [JsonProperty("codigoIBGECidade")]
        public string CodigoIBGECidade { get; set; }

        [JsonProperty("usuarioWebServer")]
        public string UsuarioWebServer { get; set; }

        [JsonProperty("senhaWebServer")]
        public string SenhaWebServer { get; set; }

        [JsonProperty("chaveAutenticacao")]
        public string ChaveAutenticacao { get; set; }

        [JsonProperty("autorizacao")]
        public string Autorizacao { get; set; }

        #endregion
    }
}
