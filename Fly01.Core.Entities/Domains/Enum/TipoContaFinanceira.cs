﻿using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoContaFinanceira
    {
        [Subtitle("ContaPagar", "Conta Pagar")]
        ContaPagar = 1,

        [Subtitle("ContaReceber", "Conta Receber")]
        ContaReceber = 2,
    }
}