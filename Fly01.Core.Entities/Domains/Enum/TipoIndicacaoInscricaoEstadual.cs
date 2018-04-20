using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
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