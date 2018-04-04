﻿using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoRegimeEspecialTrib
    {
        [Subtitle("MicroempresaMunicipal", "Microempresa Municipal")]
        MicroempresaMunicipal = 1,
        [Subtitle("MEI", "Microempresário Individual (MEI)")]
        MEI = 2,
        [Subtitle("MEEPP", "Microempresário e Empesa de Pequeno Porte (ME EPP)")]
        MEEPP = 3,
        [Subtitle("SociedadeProfissionais", "Sociedade de Profissionais")]
        SociedadeProfissionais = 4,
        [Subtitle("Imune", "Cooperativa")]
        Imune = 5,
        [Subtitle("ForaMunicípio", "Estimativa")]
        ForaMunicípio = 6,
    }
}