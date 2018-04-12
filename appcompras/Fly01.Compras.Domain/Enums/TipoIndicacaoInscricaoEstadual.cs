﻿using Fly01.Core.Entities.Attribute;

namespace Fly01.Compras.Domain.Enums
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