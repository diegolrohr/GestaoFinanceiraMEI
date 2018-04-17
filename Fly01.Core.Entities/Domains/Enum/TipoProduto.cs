﻿using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoProduto
    {
        [Subtitle("Insumo", "INSUMO", "INSUMO", "orange")]
        Insumo = 1,

        [Subtitle("ProdutoFinal", "PRODUTO FINAL", "PRODUTO FINAL", "green")]
        ProdutoFinal = 2,

        [Subtitle("Servico", "SERVIÇO", "SERVIÇO", "red")]
        Servico = 3,

        [Subtitle("Outros", "OUTROS", "OUTROS", "gray")]
        Outros = 4
    }
}