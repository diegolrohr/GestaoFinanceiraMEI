﻿using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFS
{
    public class Construcao
    {
        [XmlElement(ElementName = "codigoobra")]
        public string CodigoObra { get; set; }

        /// <summary>
        /// Anotação de Responsabilidade Técnica
        /// </summary>
        [XmlElement(ElementName = "art")]
        public string ArtObra { get; set; }

        //TODO: ver valores possiveis enum?
        [XmlElement(ElementName = "tipoobra")]
        public string TipoObra { get; set; }

        public bool ShouldSerializeTipoObra()
        {
            return !string.IsNullOrEmpty(TipoObra);
        }
    }
}
