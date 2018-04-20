using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class InformacoesAdicionais
    {
        /// <summary>
        /// informar as informações de interesse do Fisco
        /// </summary>
        [XmlElement(ElementName = "infAdFisco", IsNullable = true)]
        public string InfoInteresseDoFisico { get; set; }
        public bool ShouldSerializeInfoInteresseDoFisico()
        {
            return InfoInteresseDoFisico != null && InfoInteresseDoFisico.Length > 0;
        }

        /// <summary>
        /// informar as informações complementares de interesse do contribuite
        /// </summary>
        [XmlElement(ElementName = "infCpl", IsNullable = true)]
        public string InformacoesComplementares { get; set; }
        public bool ShouldSerializeInformacoesComplementares()
        {
            return InformacoesComplementares != null && InformacoesComplementares.Length > 0;
        }

        /// <summary>
        /// informar o grupo com o obsCont
        /// </summary>
        [XmlElement(ElementName = "obsCont")]
        public ObservacaoContribuinte ObservacaoContribuinte { get; set; }

        /// <summary>
        /// informar o grupo com o obsFisco
        /// </summary>
        [XmlElement(ElementName = "obsFisco")]
        public ObservacaoDoFisco ObservacaodoFisco { get; set; }

        /// <summary>
        /// informar o grupo com o procRef
        /// </summary>
        [XmlElement(ElementName = "procRef")]
        public ProcessoReferenciado ProcessoReferenciado { get; set; }
    }
}
