using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class InformacoesAdicionais
    {
        /// <summary>
        /// informar as informações de interesse do Fisco
        /// </summary>
        [XmlElement(ElementName = "infAdFisco")]
        public string InfoInteresseDoFisico { get; set; }

        /// <summary>
        /// informar as informações complementares de interesse do contribuite
        /// </summary>
        [XmlElement(ElementName = "infCpl")]
        public string InformacoesComplementares { get; set; }

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
