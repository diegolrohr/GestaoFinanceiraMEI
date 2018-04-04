using Fly01.EmissaoNFE.Domain.Enums;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe.IPI
{
    public abstract class IPI
    {
        /// <summary>
        /// Informar o Código de Situação Tributária do IPI.
        /// </summary>
        [XmlElement(ElementName = "CST")]
        public CSTIPI CodigoST { get; set; }        
    }
}