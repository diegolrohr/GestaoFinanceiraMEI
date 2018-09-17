using Fly01.EmissaoNFE.Domain.Entities.NFs;
using Fly01.EmissaoNFE.Domain.ViewModel;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModelNfs
{
    [XmlRoot(ElementName = "rps")]
    public class TransmissaoNFSVM : EntidadeVM
    {
        /// <summary>
        /// ID da nota, sigla e série concatenados
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string NotaId
        {
            get
            {
                return Identificacao.SerieRPS + ":" + Identificacao.NumeroRPS;
            }
            set
            {
            }
        }

        /// <summary>
        /// Versão TSS
        /// </summary>
        [XmlAttribute(AttributeName = "tssversao")]
        public string Versao
        {
            get
            {
                return "2.00";
            }
            set
            {
            }
        }

        [XmlElement(ElementName = "identificacao")]
        public Identificacao Identificacao { get; set; }

        [XmlElement(ElementName = "atividade")]
        public Atividade Atividade { get; set; }

        [XmlElement(ElementName = "prestador")]
        public Prestador Prestador { get; set; }

        [XmlElement(ElementName = "prestacao")]
        public Prestacao Prestacao { get; set; }

        [XmlElement(ElementName = "tomador")]
        public Tomador Tomador { get; set; }

        [XmlElement(ElementName = "servicos")]
        public List<Servico> Servicos { get; set; }

        [XmlElement(ElementName = "valores")]
        public Valores Valores { get; set; }

        [XmlElement(ElementName = "infcompl")]
        public InformacoesComplementares InformacoesComplementares { get; set; }
    }
}
