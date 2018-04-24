﻿using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    [XmlRoot(ElementName = "NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFeVM
    {
        [XmlElement(ElementName = "infNFe")]
        public InfoNFe InfoNFe { get; set; }
    }
}