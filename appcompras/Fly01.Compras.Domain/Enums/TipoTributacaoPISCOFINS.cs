using Fly01.Core.Helpers;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoTributacaoPISCOFINS
    {
        [Subtitle("T01", "01 - Operação Tributável com Alíquota Básica")]
        T01 = 01,
        [Subtitle("T02", "02 - Operação Tributável com Alíquota Diferenciada")]
        T02 = 02,
        [Subtitle("T03", "03 - Operação Tributável com Alíquota por Unidade de Medida de Produto")]
        T03 = 03,
        [Subtitle("T04", "04 - Operação Tributável Monofásica - Revenda a Alíquota Zero")]
        T04 = 04,
        [Subtitle("T05", "05 - Operação Tributável por Substituição Tributária")]
        T05 = 05,
        [Subtitle("T06", "06 - Operação Tributável a Alíquota Zero")]
        T06 = 06,
        [Subtitle("T07", "07 - Operação Isenta da Contribuição")]
        T07 = 07,
        [Subtitle("T08", "08 - Operação sem Incidência da Contribuição")]
        T08 = 08,
        [Subtitle("T09", "09 - Operação com Suspensão da Contribuição")]
        T09 = 09,
        [Subtitle("T49", "49 - Outras Operações de Saída")]
        T49 = 49,
        [Subtitle("T50", "50 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        T50 = 50,
        [Subtitle("T51", "51 - Operação com Direito a Crédito – Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno")]
        T51 = 51,
        [Subtitle("T52", "52 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação")]
        T52 = 52,
        [Subtitle("T53", "53 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        T53 = 53,
        [Subtitle("T54", "54 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        T54 = 54,
        [Subtitle("T55", "55 - Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        T55 = 55,
        [Subtitle("T56", "56 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        T56 = 56,
        [Subtitle("T60", "60 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        T60 = 60,
        [Subtitle("T61", "61 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno")]
        T61 = 61,
        [Subtitle("T62", "62 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação")]
        T62 = 62,
        [Subtitle("T63", "63 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        T63 = 63,
        [Subtitle("T64", "64 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        T64 = 64,
        [Subtitle("T65", "65 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        T65 = 65,
        [Subtitle("T66", "66 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        T66 = 66,
        [Subtitle("T67", "67 - Crédito Presumido - Outras Operações")]
        T67 = 67,
        [Subtitle("T70", "70 - Operação de Aquisição sem Direito a Crédito")]
        T70 = 70,
        [Subtitle("T71", "71 - Operação de Aquisição com Isenção")]
        T71 = 71,
        [Subtitle("T72", "72 - Operação de Aquisição com Suspensão")]
        T72 = 72,
        [Subtitle("T73", "73 - Operação de Aquisição a Alíquota Zero")]
        T73 = 73,
        [Subtitle("T74", "74 - Operação de Aquisição sem Incidência da Contribuição")]
        T74 = 74,
        [Subtitle("T75", "75 - Operação de Aquisição por Substituição Tributária")]
        T75 = 75,
        [Subtitle("T98", "98 - Outras Operações de Entrada")]
        T98 = 98,
        [Subtitle("T99", "99 - Outras Operações")]
        T99 = 99
    }
}