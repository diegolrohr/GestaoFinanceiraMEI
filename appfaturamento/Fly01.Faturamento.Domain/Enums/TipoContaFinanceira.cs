﻿using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoContaFinanceira
    {
        [Subtitle("ContaPagar", "Conta Pagar")]
        ContaPagar = 1,
        [Subtitle("ContaReceber", "Conta Receber")]
        ContaReceber = 2,
    }
}
