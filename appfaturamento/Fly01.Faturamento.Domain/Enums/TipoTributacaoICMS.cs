using Fly01.Core.Helpers;

namespace Fly01.Faturamento.Domain.Enums
{
    public enum TipoTributacaoICMS
    {
        [Subtitle("TributadaComPermissaoDeCredito", "101 - Tributada pelo Simples Nacional com permissão de crédito")]
        TributadaComPermissaoDeCredito = 101,
        [Subtitle("TributadaSemPermissaoDeCredito", "102 - Tributada pelo Simples Nacional sem permissão de crédito")]
        TributadaSemPermissaoDeCredito = 102,
        [Subtitle("IsencaoParaFaixaDeReceitaBruta", "103 - Isenção do ICMS no Simples Nacional para faixa de receita bruta")]
        IsencaoParaFaixaDeReceitaBruta = 103,
        [Subtitle("TributadaComPermissaoDeCreditoST", "201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por substituição tributária")]
        TributadaComPermissaoDeCreditoST = 201,
        [Subtitle("TributadaSemPermissaoDeCreditoST", "202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por substituição tributária")]
        TributadaSemPermissaoDeCreditoST = 202,
        [Subtitle("IsencaoParaFaixaDeReceitaBrutaST", "203 - Isenção do ICMS no Simples Nacional para faixa de receita bruta e com cobrança do ICMS por substituição tributária")]
        IsencaoParaFaixaDeReceitaBrutaST = 203,
        [Subtitle("Imune", "300 - Imune")]
        Imune = 300,
        [Subtitle("NaoTributadaPeloSN", "400 - Não tributada pelo Simples Nacional")]
        NaoTributadaPeloSN = 400,
        [Subtitle("ICMSSubstituidoOuAntecipado", "500 - ICMS cobrado anteriormente por substituição tributária (substituído) ou por antecipação")]
        ICMSSubstituidoOuAntecipado = 500,
        [Subtitle("Outros", "900 - Outros")]
        Outros = 900
    }
}