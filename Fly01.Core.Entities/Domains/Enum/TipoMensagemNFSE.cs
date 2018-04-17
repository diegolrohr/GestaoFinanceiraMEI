﻿using Fly01.Core.Entities.Attribute;

namespace Fly01.Core.Entities.Domains.Enum
{
    public enum TipoMensagemNFSE
    {
        [Subtitle("DP", "Descricao do Produto")]
        DP = 1,

        [Subtitle("DPIMNF", "Descricao do Produto + Imposto + Mensagem na NF")]
        DPIMNF = 2,

        [Subtitle("IMNF", "Imposto + Mensagem na NF")]
        IMNF = 3,

        [Subtitle("DPDAMNF", "Descricao do Produto + Descrição Auxiliar + Mensagem na NF")]
        DPDAMNF = 4
    }
}