using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoModalidade
    {
        [Subtitle("Normal", "Emissão Normal")]
        Normal = 1,
        [Subtitle("FSIA", "Contingência FS-IA, com impressão do DANFE em formulário de segurança")]
        FSIA = 2,
        [Subtitle("SCAN", "Sistema de Contingência do Ambiente Nacional")]
        SCAN = 3,
        [Subtitle("DPEC", "Declaração Prévia da Emissão em Contingência")]
        DPEC = 4,
        [Subtitle("FSDA", "Contingência FS-DA, com impressão do DANFE em formulário de segurança")]
        FSDA = 5,
        [Subtitle("SVCAN", "SEFAZ Virtual de Contingência do AN")]
        SVCAN = 6,
        [Subtitle("SVCRS", "SEFAZ Virtual de Contingência do RS")]
        SVCRS = 7,
        [Subtitle("NFCe", "Contingência off-line da NFC-e")]
        NFCe = 9
    }
}
