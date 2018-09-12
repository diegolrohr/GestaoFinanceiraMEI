using Fly01.EmissaoNFE.Domain.Enums;
using System;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFs
{
    public class Identificacao
    {
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
        /// O tipo é fixo, pois tanto ABRASF como DSFNET, utilizam esta tag como tipo RPS (1)
        /// </summary>
        [XmlElement(ElementName = "tipo")] //TipoNFs
        public string TipoNFs {
            get
            {
                 return "1";
            }
            set { }
        }

        /// <summary>
        /// O tipo é fixo, pois tanto ABRASF como DSFNET, utilizam esta tag como Normal (1)
        /// </summary>
        [XmlElement(ElementName = "situacaorps")]
        public string TipoSituacaoRPS
        {
            get
            {
                return "1";
            }
            set { }
        }

        [XmlElement(ElementName = "tiporecolhe")]
        public TipoRecolhimentoNFs TipoRecolhimento { get; set; }

        [XmlElement(ElementName = "tipooper")]
        public TipoOperacaoNFs TipoOperacao { get; set; }

        [XmlElement(ElementName = "tipotrib")]
        public TipoTributacaoNFs TipoTributacao{ get; set; }

        [XmlElement(ElementName = "regimeesptrib")]
        public TipoRegimeEspecialTributacaoNFs TipoRegimeEspecialTributacao { get; set; }

        [XmlElement(ElementName = "deveissmunprestador")]
        public TipoISSMunicipioPrestadorNFs TipoISSMunicipioPrestador { get; set; }
    }
}