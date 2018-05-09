using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum StatusArquivoRemessa
    {
        [Subtitle("Exportado", "Exportado")]
        Exportado = 1,

        [Subtitle("AguardandoAprovacao", "Aguardando Aprovação")]
        AguardandoAprovacao = 2
    }
}