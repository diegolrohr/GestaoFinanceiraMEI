using Fly01.Core.Entities.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    /// <summary>
    /// Tabela de CST do PIS: Código da Situação Tributária referente ao PIS/PASEP
    /// </summary>
    public enum CSTPISCOFINS
    {
        [XmlEnum(Name = "01")]
        [Subtitle("TributavelComAliquotaBasica", "01", "Operação Tributável com Alíquota Básica")]
        TributavelComAliquotaBasica = 1,

        [XmlEnum(Name = "02")]
        [Subtitle("TributavelComAliquotaDiferenciada", "02", "Operação Tributável com Alíquota Diferenciada")]
        TributavelComAliquotaDiferenciada = 2,

        [XmlEnum(Name = "03")]
        [Subtitle("TributavelComAliquotaPorUMDeProduto", "03", "Operação Tributável com Alíquota por Unidade de Medida de Produto")]
        TributavelComAliquotaPorUMDeProduto = 3,

        [XmlEnum(Name = "04")]
        [Subtitle("TributavelComAliquotaPorUMDeProduto", "04", "Operação Tributável Monofásica - Revenda a Alíquota Zero")]
        TributavelMonofasica = 4,

        [XmlEnum(Name = "05")]
        [Subtitle("TributavelPorST", "05", "Operação Tributável por Substituição Tributária")]
        TributavelPorST = 5,

        [XmlEnum(Name = "06")]
        [Subtitle("TributavelAliquotaZero", "06", "Operação Tributável a Alíquota Zero")]
        TributavelAliquotaZero = 6,

        [XmlEnum(Name = "07")]
        [Subtitle("IsentaDaContribuicao", "07", "Operação Isenta da Contribuição")]
        IsentaDaContribuicao = 7,

        [XmlEnum(Name = "08")]
        [Subtitle("SemIncidencia", "08", "Operação sem Incidência da Contribuição")]
        SemIncidencia = 8,

        [XmlEnum(Name = "09")]
        [Subtitle("ComSuspensao", "09", "Operação com Suspensão da Contribuição")]
        ComSuspensao = 9,

        [XmlEnum(Name = "49")]
        [Subtitle("OutrasOperacoesDeSaida", "49", "Outras Operações de Saída")]
        OutrasOperacoesDeSaida = 49,

        [XmlEnum(Name = "50")]
        [Subtitle("ReceitaTributadaNoMercadoInterno", "50", "Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        ReceitaTributadaNoMercadoInterno = 50,

        [XmlEnum(Name = "51")]
        [Subtitle("ReceitaNaoTributadaNoMercadoInterno", "51", "Operação com Direito a Crédito – Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno")]
        ReceitaNaoTributadaNoMercadoInterno = 51,

        [XmlEnum(Name = "52")]
        [Subtitle("ReceitaDeExportacao", "52", "Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação")]
        ReceitaDeExportacao = 52,

        [XmlEnum(Name = "53")]
        [Subtitle("ReceitaNoMercadoInterno", "53", "Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        ReceitaNoMercadoInterno = 53,

        [XmlEnum(Name = "54")]
        [Subtitle("ReceitaTributadaNoMercadoInternoEExportacao", "54", "Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        ReceitaTributadaNoMercadoInternoEExportacao = 54,

        [XmlEnum(Name = "55")]
        [Subtitle("ReceitaNaoTributadaNoMercadoInternoEExportacao", "55", "Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        ReceitaNaoTributadaNoMercadoInternoEExportacao = 55,

        [XmlEnum(Name = "56")]
        [Subtitle("ReceitaNoMercadoInternoEExportacao", "56", "Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        ReceitaNoMercadoInternoEExportacao = 56,

        [XmlEnum(Name = "60")]
        [Subtitle("AquisicaoTributadaNoMercadoInterno", "60", "Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        AquisicaoTributadaNoMercadoInterno = 60,

        [XmlEnum(Name = "61")]
        [Subtitle("AquisicaoNaoTributadaNoMercadoInterno", "61", "Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno")]
        AquisicaoNaoTributadaNoMercadoInterno = 61,

        [XmlEnum(Name = "62")]
        [Subtitle("AquisicaoReceitaExportacao", "62", "Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação")]
        AquisicaoReceitaExportacao = 62,

        [XmlEnum(Name = "63")]
        [Subtitle("AquisicaoReceitaMercadoInterno", "63", "Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        AquisicaoReceitaMercadoInterno = 63,

        [XmlEnum(Name = "64")]
        [Subtitle("AquisicaoTributadaMercadoInternoEExportacao", "64", "Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        AquisicaoTributadaMercadoInternoEExportacao = 64,

        [XmlEnum(Name = "65")]
        [Subtitle("AquisicaoNaoTributadaMercadoInternoEExportacao", "65", "Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        AquisicaoNaoTributadaMercadoInternoEExportacao = 65,

        [XmlEnum(Name = "66")]
        [Subtitle("AquisicaoMercadoInternoEExportacao", "66", "Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        AquisicaoMercadoInternoEExportacao = 66,

        [XmlEnum(Name = "67")]
        [Subtitle("CreditoPresumidoOutrasOperacoes", "67", "Crédito Presumido - Outras Operações")]
        CreditoPresumidoOutrasOperacoes = 67,

        [XmlEnum(Name = "70")]
        [Subtitle("AquisicaoSemCredito", "70", "Operação de Aquisição sem Direito a Crédito")]
        AquisicaoSemCredito = 70,

        [XmlEnum(Name = "71")]
        [Subtitle("AquisicaoComIsencao", "71", "Operação de Aquisição com Isenção")]
        AquisicaoComIsencao = 71,

        [XmlEnum(Name = "72")]
        [Subtitle("AquisicaoComSuspensao", "72", "Operação de Aquisição com Suspensão")]
        AquisicaoComSuspensao = 72,

        [XmlEnum(Name = "73")]
        [Subtitle("AquisicaoAliquotaZero", "73", "Operação de Aquisição a Alíquota Zero")]
        AquisicaoAliquotaZero = 73,

        [XmlEnum(Name = "74")]
        [Subtitle("AquisicaoSemIncidencia", "74", "Operação de Aquisição sem Incidência da Contribuição")]
        AquisicaoSemIncidencia = 74,

        [XmlEnum(Name = "75")]
        [Subtitle("AquisicaoPorST", "75", "Operação de Aquisição por Substituição Tributária")]
        AquisicaoPorST = 75,

        [XmlEnum(Name = "98")]
        [Subtitle("OutrasOperacoesDeEntrada", "98", "Outras Operações de Entrada")]
        OutrasOperacoesDeEntrada = 98,

        [XmlEnum(Name = "99")]
        [Subtitle("OutrasOperacoes", "99", "Outras Operações")]
        OutrasOperacoes = 99

    }
}
