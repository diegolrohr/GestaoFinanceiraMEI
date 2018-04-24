﻿using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoAmbiente
    {
        [XmlEnum(Name = "0")]
        [Subtitle("Configuracao", "0", "Configuração")]
        Configuracao = 0,

        [XmlEnum(Name = "1")]
        [Subtitle("Producao", "1", "Produção")]
        Producao = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Homologacao", "2", "Homologação")]
        Homologacao = 2
    }
}