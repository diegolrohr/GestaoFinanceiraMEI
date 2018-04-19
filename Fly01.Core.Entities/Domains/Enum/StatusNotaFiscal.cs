﻿using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusNotaFiscal
    {
        [XmlEnum(Name = "1")]
        [Subtitle("Transmitida", "Transmitida", "Transmitida", "blue")]
        Transmitida = 1,

        [XmlEnum(Name = "2")]
        [Subtitle("Autorizada", "Autorizada", "Autorizada", "green")]
        Autorizada = 2,

        [XmlEnum(Name = "2")]
        [Subtitle("UsoDenegado", "Uso Denegado", "Uso Denegado", "orange")]
        UsoDenegado = 3,

        [XmlEnum(Name = "2")]
        [Subtitle("NaoAutorizada", "Não Autorizada", "Não Autorizada", "red")]
        NaoAutorizada = 4,

        [XmlEnum(Name = "2")]
        [Subtitle("Cancelada", "Cancelada", "Cancelada", "black")]
        Cancelada = 5,

        [XmlEnum(Name = "2")]
        [Subtitle("NaoTransmitida", "Não Transmitida", "Não Transmitida", "gray")]
        NaoTransmitida = 6,

        [XmlEnum(Name = "2")]
        [Subtitle("EmCancelamento", "Em Cancelamento", "Em Cancelamento", "brown")]
        EmCancelamento = 7,

        [XmlEnum(Name = "2")]
        [Subtitle("FalhaNoCancelamento", "Falha no Cancelamento", "Falha no Cancelamento", "pink")]
        FalhaNoCancelamento = 8,
        
        [XmlEnum(Name = "2")]
        [Subtitle("FalhaTransmissao", "Falha na Transmissão", "Falha na Transmissão", "red")]
        FalhaTransmissao = 9,
        
        [XmlEnum(Name = "2")]
        [Subtitle("CanceladaForaPrazo", "Cancelada fora do prazo", "Cancelada fora do prazo", "black")]
        CanceladaForaPrazo = 10
    }
}