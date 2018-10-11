﻿using System;
using Newtonsoft.Json;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    [Serializable]
    public class ParametroTributarioVM : DomainBaseVM
    {
        [JsonProperty("simplesNacional")]
        public bool SimplesNacional { get; set; }

        [JsonProperty("aliquotaSimplesNacional")]
        public double AliquotaSimplesNacional { get; set; }

        [JsonProperty("aliquotaISS")]
        public double AliquotaISS { get; set; }

        [JsonProperty("aliquotaPISPASEP")]
        public double AliquotaPISPASEP { get; set; }

        [JsonProperty("aliquotaCOFINS")]
        public double AliquotaCOFINS { get; set; }

        [JsonProperty("aliquotaCSLL")]
        public double AliquotaCSLL { get; set; }

        [JsonProperty("aliquotaINSS")]
        public double AliquotaINSS { get; set; }

        [JsonProperty("aliquotaImpostoRenda")]
        public double AliquotaImpostoRenda { get; set; }

        [JsonProperty("registroSimplificadoMT")]
        public bool RegistroSimplificadoMT { get; set; }

        [JsonProperty("mensagemPadraoNota")]
        public string MensagemPadraoNota { get; set; }

        [JsonProperty("numeroRetornoNF")]
        public string NumeroRetornoNF { get; set; }

        [JsonProperty("tipoAmbiente")]
        [APIEnum("TipoAmbiente")]
        public string TipoAmbiente { get; set; }

        [JsonProperty("tipoVersaoNFe")]
        [APIEnum("TipoVersaoNFe")]
        public string TipoVersaoNFe { get; set; }

        [JsonProperty("tipoModalidade")]
        [APIEnum("TipoModalidade")]
        public string TipoModalidade { get; set; }

        [JsonProperty("aliquotaFCP")]
        public double AliquotaFCP { get; set; }

        [JsonProperty("tipoPresencaComprador")]
        [APIEnum("TipoPresencaComprador")]
        public string TipoPresencaComprador { get; set; }

        [JsonProperty("horarioVerao")]
        [APIEnum("HorarioVerao")]
        public string HorarioVerao { get; set; }

        [JsonProperty("tipoHorario")]
        [APIEnum("TipoHorario")]
        public string TipoHorario { get; set; }

        #region NFS
        [JsonProperty("versaoNFSe")]
        public string VersaoNFSe { get; set; }

        [JsonProperty("tipoAmbienteNFS")]
        [APIEnum("TipoAmbiente")]
        public string TipoAmbienteNFS { get; set; }

        [JsonProperty("incentivoCultura")]
        public bool IncentivoCultura { get; set; }

        [JsonProperty("usuarioWebServer")]
        public string UsuarioWebServer { get; set; }

        [JsonProperty("senhaWebServer")]
        public string SenhaWebServer { get; set; }

        [JsonProperty("chaveAutenticacao")]
        public string ChaveAutenticacao { get; set; }

        [JsonProperty("autorizacao")]
        public string Autorizacao { get; set; }

        [JsonProperty("tipoTributacaoNFS")]
        [APIEnum("TipoTributacaoNFS")]
        public string TipoTributacaoNFS { get; set; }

        [JsonProperty("formatarCodigoISS")]
        public bool FormatarCodigoISS { get; set; }

        #endregion
    }
}