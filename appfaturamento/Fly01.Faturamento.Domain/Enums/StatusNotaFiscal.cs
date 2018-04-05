using Fly01.Core.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum StatusNotaFiscal
    {
        [Subtitle("Transmitida", "Transmitida", "Transmitida", "blue")]
        Transmitida = 1,

        [Subtitle("Autorizada", "Autorizada", "Autorizada", "green")]
        Autorizada = 2,

        [Subtitle("UsoDenegado", "Uso Denegado", "Uso Denegado", "orange")]
        UsoDenegado = 3,

        [Subtitle("NaoAutorizada", "Não Autorizada", "Não Autorizada", "red")]
        NaoAutorizada = 4,

        [Subtitle("Cancelada", "Cancelada", "Cancelada", "black")]
        Cancelada = 5,

        [Subtitle("NaoTransmitida", "Não Transmitida", "Não Transmitida", "gray")]
        NaoTransmitida = 6,

        [Subtitle("EmCancelamento", "Em Cancelamento", "Em Cancelamento", "brown")]
        EmCancelamento = 7,

        [Subtitle("FalhaNoCancelamento", "Falha no Cancelamento", "Falha no Cancelamento", "pink")]
        FalhaNoCancelamento = 8,
        
        [Subtitle("FalhaTransmissao", "9", "Falha na Transmissão", "red")]
        FalhaTransmissao = 9
    }
}