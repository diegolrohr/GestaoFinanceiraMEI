﻿using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.ViewModel
{
    [XmlRoot("detEvento")]
    public class DetalheEvento
    {
        /// <summary>
        /// Tipo evento
        /// </summary>
        [XmlElement(ElementName = "tpEvento")]
        public string TipoEvento { get; set; }

    }
}
