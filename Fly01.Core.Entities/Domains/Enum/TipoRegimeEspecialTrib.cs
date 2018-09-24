using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoRegimeEspecialTributacao
    {
        [Subtitle("MicroEmpresaMunicipal", "Micro Empresa Municipal")]
        MicroEmpresaMunicipal = 1,

        [Subtitle("MEI", "Microempresário Individual (MEI)")]
        MEI = 2,

        [Subtitle("MEEPP", "Microempresário e Empesa de Pequeno Porte (ME EPP)")]
        MEEPP = 3,

        [Subtitle("SociedadeProfissionais", "Sociedade de Profissionais")]
        SociedadeProfissionais = 4,

        [Subtitle("Imune", "Imune Cooperativa")]
        Imune = 5,

        [Subtitle("ForaMunicipio", "Fora Município Estimativa")]
        ForaMunicipio = 6,
    }
}