using Fly01.Core.Attribute;

namespace Fly01.Compras.Domain.Enums
{
    public enum TipoTributacaoICMS
    {
        [Subtitle("T101", "101 - Tributada pelo Simples Nacional com permissão de crédito")]
        T101 = 101,
        [Subtitle("T102", "102 - Tributada pelo Simples Nacional sem permissão de crédito")]
        T102 = 102,
        [Subtitle("T103", "103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta")]
        T103 = 103,
        [Subtitle("T201", "201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária")]
        T201 = 201,
        [Subtitle("T202", "202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária")]
        T202 = 202,
        [Subtitle("T203", "203 - Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária")]
        T203 = 203,
        [Subtitle("T300", "300 - Imune")]
        T300 = 300,
        [Subtitle("T400", "400 - Não tributada pelo Simples Nacional")]
        T400 = 400,
        [Subtitle("T500", "500 - ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação")]
        T500 = 500,
        [Subtitle("T900", "900 - Outros")]
        T900 = 900
    }
}