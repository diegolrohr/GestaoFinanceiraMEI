using Fly01.EmissaoNFE.Domain.Entities.NFS;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModelNFS
{
    [XmlRoot(ElementName = "rps")]
    public class ItemTransmissaoNFSVM
    {
        /// <summary>
        /// ID da nota, sigla e série concatenados
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string NotaId
        {
            get
            {
                return Identificacao.SerieRPS.ToLower() + ":" + Identificacao.NumeroRPS;
            }
            set { }
        }



        private string versao = "2.00";
        /// <summary>
        /// Versão TSS
        /// </summary>
        [XmlAttribute(AttributeName = "tssversao")]
        public string Versao
        {
            get
            {
                return versao;
            }
            set
            {
                versao = value;
            }
        }
        /// <summary>
        /// Gerado pelo sistema na serialização do xml
        /// </summary>
        [XmlElement(ElementName = "assinatura")]
        public string Assinatura { get; set; }

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

        [XmlArray("servicos"), XmlArrayItem(typeof(Servico), ElementName = "servico")]
        public List<Servico> Servicos { get; set; }

        /// <summary>
        /// Gerado pelo sistema na serialização do xml
        /// </summary>
        [XmlElement(ElementName = "valores")]
        public Valores Valores { get; set; }

        [XmlElement(ElementName = "infcompl")]
        public InformacoesComplementares InformacoesComplementares { get; set; }

    }
}
