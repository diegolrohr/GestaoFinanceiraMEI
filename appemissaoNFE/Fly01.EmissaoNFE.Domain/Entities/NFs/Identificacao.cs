using Fly01.Core.Entities.Domains.Enum;
using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    [XmlRoot(ElementName = "rps")]
    public class Identificacao
    {
        [XmlIgnore]
        public string CodigoIBGEPrestador { get; set; }

        [XmlIgnore]
        public DateTime DataHoraEmissao { get; set; }

        [XmlElement(ElementName = "dthremissao")]
        public string DataHoraEmissaoString
        {
            get { return DataHoraEmissao.ToString("yyyy-MM-ddTHH:mm:sszzzz"); }
            set { DataHoraEmissao = DateTime.Parse(value); }
        }

        [XmlElement(ElementName = "serierps")]
        public string SerieRPS { get; set; }

        [XmlElement(ElementName = "numerorps")]
        public int NumeroRPS { get; set; }

        /// <summary>
        /// O tipo é fixo como RPS (1)
        /// </summary>
        [XmlElement(ElementName = "tipo")] //TipoNFs
        public TipoSimNao TipoNFs
        {
            get
            {
                 return TipoSimNao.Sim;
            }
        }

        /// <summary>
        /// O tipo é fixo, pois tanto ABRASF como DSFNET, utilizam esta tag como Normal (1)
        /// </summary>
        [XmlElement(ElementName = "situacaorps")]
        public TipoSimNao TipoSituacaoRPS
        {
            get
            {
                return TipoSimNao.Sim;
            }
        }

        /// <summary>
        /// O tipo é fixo pois sempre recolhe ISS
        /// </summary>
        [XmlElement(ElementName = "tiporecolhe")]
        public TipoSimNao TipoRecolhimento
        {
            get
            {
                return TipoSimNao.Sim;
            }
        }


        /// <summary>
        /// O tipo é fixo, pois tanto é uma saida e o default é 1
        /// </summary>
        [XmlElement(ElementName = "tipooper")]
        public TipoSimNao TipoOperacao
        {
            get
            {
                return TipoSimNao.Sim;
            }
        }

        [XmlElement(ElementName = "tipotrib")]
        public TipoTributacaoNFS TipoTributacao{ get; set; }

        /// <summary>
        /// 1 true 2 false
        /// </summary>
        [XmlElement(ElementName = "regimeesptrib")]
        public string TipoRegimeEspecialTributacao {
            get
            {
                return TipoTributacao == TipoTributacaoNFS.CreditaZonaFrancaManaus ? "1" : "2";
            }
        }

        /// <summary>
        /// O tipo é fixo pois sempre deve ISS ao municipio
        /// </summary>
        [XmlElement(ElementName = "deveissmunprestador")]
        public TipoSimNao TipoISSMunicipioPrestador
        {
            get
            {
                return TipoSimNao.Sim;
            }   
        }

        /// <summary>
        /// Tag especifica para cidade Vitória - Espirito Santo
        /// </summary>
        [XmlIgnore]
        public DateTime CompetenciaRPS { get; set; }

        [XmlElement(ElementName = "competenciarps")]
        public string CompetenciaRPSString
        {
            get { return CompetenciaRPS.ToString("yyyy-MM-dd"); }
            set { CompetenciaRPS = DateTime.Parse(value); }
        }

        public bool ShouldSerializeCompetenciaRPSString()
        {
            return (!string.IsNullOrEmpty(CodigoIBGEPrestador) && CodigoIBGEPrestador.ToUpper() == "3205309");
        }
    }
}