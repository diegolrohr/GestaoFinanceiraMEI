﻿using Fly01.Core.Helpers.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoOrdemCompra
    {
        [Subtitle("Orcamento", "Orçamento", "Orçamento", "blue")]
        Orcamento = 1,

        [Subtitle("Pedido", "Pedido", "Pedido", "brown")]
        Pedido = 2
    }
}