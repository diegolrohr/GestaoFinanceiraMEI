using Fly01.Core.Entities.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoAmbiente
    {
        [Subtitle("Configuracao", "Configuração")]
        Configuracao = 0,
        [Subtitle("Producao", "Produção")]
        Producao = 1,
        [Subtitle("Homologacao", "Homologação")]
        Homologacao = 2
    }
}