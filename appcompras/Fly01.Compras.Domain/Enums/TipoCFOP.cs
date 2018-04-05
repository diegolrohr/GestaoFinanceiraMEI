﻿using Fly01.Core.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoCFOP
    {
        [Subtitle("Entrada", "Entrada")]
        Entrada = 1,
        [Subtitle("Saida", "Saída")]
        Saida = 2,
    }
}