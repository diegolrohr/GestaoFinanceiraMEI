using Fly01.Core.Attribute;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoIndicacaoInscricaoEstadual
    {
        [Subtitle("ContribuinteICMS", "Contribuinte ICMS")]
        ContribuinteICMS = 1,
        [Subtitle("ContribuinteIsento", "Contribuinte Isento")]
        ContribuinteIsento = 2,
        [Subtitle("NaoContribuinte", "Não Contribuinte")]
        NaoContribuinte = 9
    }
}
