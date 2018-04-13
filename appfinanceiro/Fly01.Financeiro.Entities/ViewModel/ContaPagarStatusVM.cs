using System;
using Fly01.Core.Entities.Attribute;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public enum ContaPagarStatusVM
    {
        [Subtitle("TituloEmAberto", "Em aberto", "ABER", "orange")]
        TituloEmAberto = 1,

        [Subtitle("TituloBaixado", "Pago", "PAGO", "green")]
        TituloBaixado = 2,

        [Subtitle("BaixadoParcialmente", "Baixado Parcialmente", "BPAR", "gray")]
        BaixadoParcialmente = 3,

        [Subtitle("TituloRenegociado", "Renegociado", "RENG", "red")]
        TituloRenegociado = 4,

        [Subtitle("TituloBaixadoPorRenegociacao", "Baixado por Renegociação", "BXRN", "brown")]
        TituloBaixadoPorRenegociacao = 5,

        [Subtitle("AdiantamentoComSaldo", "Adiantamento com Saldo", "ADCS", "silver")]
        AdiantamentoComSaldo = 6,

        [Subtitle("TituloBloqueado", "Bloqueado", "BLOQ", "pink")]
        TituloBloqueado = 7,

        [Subtitle("TituloCabecalho", "Cabeçalho", "CABE", "pink")]
        TituloCabecalho = 8,

        [Subtitle("TituloCabecalhoBaixadoParcialmente", "Cabeçalho Baixado Parcialmente", "CBPA", "pink")]
        TituloCabecalhoBaixadoParcialmente = 9,
        
        [Subtitle("TituloCabecalhoBaixado", "Cabeçalho Baixado", "CBBX", "pink")]
        TituloCabecalhoBaixado = 10,
        
        [Subtitle("TituloChequeEmTransito", "Cheque em Trânsito", "CQTR", "blue")]
        TituloChequeEmTransito = 11,

        [Subtitle("TituloChequeNaoLiberado", "Cheque não Liberado", "CQNL", "yellow")]
        TituloChequeNaoLiberado = 12
    }
}
