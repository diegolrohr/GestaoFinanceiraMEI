using Fly01.Faturamento.Domain.Enums;
using Fly01.Core.Domain;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly01.Faturamento.Domain.Entities
{
    public class ParametroTributario : PlataformaBase
    {
        public bool SimplesNacional { get; set; }

        public double AliquotaSimplesNacional { get; set; }

        public double AliquotaFCP { get; set; }

        public double AliquotaISS { get; set; }

        public double AliquotaPISPASEP { get; set; }

        public double AliquotaCOFINS { get; set; }

        public bool RegistroSimplificadoMT { get; set; }

        public string MensagemPadraoNota { get; set; }

        public string NumeroRetornoNF { get; set; }

        [JsonIgnore]
        public TipoAmbiente TipoAmbiente { get; set; }

        [NotMapped]
        [JsonProperty("tipoAmbiente")]
        public string TipoAmbienteRest
        {
            get { return ((int)TipoAmbiente).ToString(); }
            set { TipoAmbiente = (TipoAmbiente)Enum.Parse(typeof(TipoAmbiente), value); }
        }

        [JsonIgnore]
        public TipoVersaoNFe TipoVersaoNFe { get; set; }

        [NotMapped]
        [JsonProperty("tipoVersaoNFe")]
        public string TipoVersaoNFeRest
        {
            get { return ((int)TipoVersaoNFe).ToString(); }
            set { TipoVersaoNFe = (TipoVersaoNFe)Enum.Parse(typeof(TipoVersaoNFe), value); }
        }

        [JsonIgnore]
        public TipoModalidade TipoModalidade { get; set; }

        [NotMapped]
        [JsonProperty("tipoModalidade")]
        public string TipoModalidadeRest
        {
            get { return ((int)TipoModalidade).ToString(); }
            set { TipoModalidade = (TipoModalidade)Enum.Parse(typeof(TipoModalidade), value); }
        }

        #region NFS
        //public bool IncentivoCultura { get; set; }

        //[JsonIgnore]
        //public TipoRegimeEspecialTrib TipoRegimeEspecialTrib { get; set; }

        //[NotMapped]
        //[JsonProperty("tipoRegimeEspecialTrib")]
        //public string TipoRegimeEspecialTribRest
        //{
        //    get { return ((int)TipoRegimeEspecialTrib).ToString(); }
        //    set { TipoRegimeEspecialTrib = (TipoRegimeEspecialTrib)Enum.Parse(typeof(TipoRegimeEspecialTrib), value); }
        //}

        //[JsonIgnore]
        //public TipoMensagemNFSE? TipoMensagemNFSE { get; set; }

        //[NotMapped]
        //[JsonProperty("tipoMensagemNFSE")]
        //public string TipoMensagemNFSERest
        //{
        //    get { return ((int)TipoMensagemNFSE).ToString(); }
        //    set { TipoMensagemNFSE = (TipoMensagemNFSE)Enum.Parse(typeof(TipoMensagemNFSE), value); }
        //}

        //[JsonIgnore]
        //public TipoLayoutNFSE? TipoLayoutNFSE { get; set; }

        //[NotMapped]
        //[JsonProperty("tipoLayoutNFSE")]
        //public string TipoLayoutNFSERest
        //{
        //    get { return ((int)TipoLayoutNFSE).ToString(); }
        //    set { TipoLayoutNFSE = (TipoLayoutNFSE)Enum.Parse(typeof(TipoLayoutNFSE), value); }
        //}

        //public bool NovoModeloUnicoXMLTSS { get; set; }

        //public string SIAFI { get; set; }

        //public string Usuario { get; set; }

        //public string Senha { get; set; }

        //public string ChaveAutenticacao { get; set; }
        #endregion
    }
}