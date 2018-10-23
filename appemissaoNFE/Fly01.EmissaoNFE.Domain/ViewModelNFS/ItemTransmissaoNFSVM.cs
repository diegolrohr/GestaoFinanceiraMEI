using Fly01.EmissaoNFE.Domain.Entities.NFS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [JsonIgnore]
        [XmlElement(ElementName = "assinatura")]
        public string AssinaturaHash { get; set; }

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

        //TODO: ver faturas e pagamentos, repete a cada parcela nos 2? ou agrupado
        [XmlArray("faturas"), XmlArrayItem(typeof(Fatura), ElementName = "fatura")]
        public List<Fatura> Faturas { get; set; }

        public bool ShouldSerializeFaturas()
        {
            return Faturas != null && Faturas.Any();
        }

        [XmlElement(ElementName = "pagamentos")]
        public Pagamentos Pagamentos { get; set; }

        public bool ShouldSerializePagamentos()
        {
            return Pagamentos != null && (Pagamentos.ListaPagamentos != null && Pagamentos.ListaPagamentos.Any());
        }

        //TODO:tag deducoes
        //TODO:tag construcao

        [XmlElement(ElementName = "infcompl")]
        public InformacoesComplementares InformacoesComplementares { get; set; }

    }
}
