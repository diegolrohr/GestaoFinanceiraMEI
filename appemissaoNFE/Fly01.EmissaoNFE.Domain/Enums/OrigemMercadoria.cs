using Fly01.Core.Helpers.Attribute;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum OrigemMercadoria
    {
        /// <summary>
        /// 0 - Nacional, exceto as indicadas nos códigos 3, 4, 5 e 8;
        /// </summary>
        [XmlEnum(Name = "0")]
        [Subtitle("Nacional", "0 - Nacional")]
        Nacional = 0,
        
        /// <summary>
        /// 1 - Estrangeira - Importação direta, exceto a indicada no código 6;
        /// </summary>
        [XmlEnum(Name = "1")]
        [Subtitle("EstrangeiraImportacaoDireta", "1 - Estrangeira, importação direta")]
        EstrangeiraImportacaoDireta = 1,

        /// <summary>
        /// 2 - Estrangeira - Adquirida no mercado interno, exceto a indicada no código 7;
        /// </summary>
        [XmlEnum(Name = "2")]
        [Subtitle("EstrangeiraAdquiridaMercadoInterno", "2 - Estrangeira, adquirida mercado interno")]
        EstrangeiraAdquiridaMercadoInterno = 2,

        /// <summary>
        /// 3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%;
        /// </summary>
        [XmlEnum(Name = "3")]
        [Subtitle("NacionalConteudoImportacaoEntre40e70", "3 - Nacional, com conteúdo importação entre 40% e 70%")]
        NacionalConteudoImportacaoEntre40e70 = 3,

        /// <summary>
        /// 4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam as legislações citadas nos Ajustes;
        /// </summary>
        [XmlEnum(Name = "4")]
        [Subtitle("NacionalProducaoEmConformidade", "4 - Nacional, produção em conformidade os processos produtivos básicos")]
        NacionalProducaoEmConformidade = 4,

        /// <summary>
        /// 5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%;
        /// </summary>
        [XmlEnum(Name = "5")]
        [Subtitle("NacionalConteudoImportacaoInferior40", "5 - Nacional, conteúdo importação inferior ou igual à 40%")]
        NacionalConteudoImportacaoInferior40 = 5,

        /// <summary>
        /// 6 - Estrangeira - Importação direta, sem similar nacional, constante em lista da CAMEX e gás natural;
        /// </summary>
        [XmlEnum(Name = "6")]
        [Subtitle("EstrangeiraImportacaoDiretaSemSimilarNacional", "6 - Estrangeira, importação direta sem similar nacional, consta lista CAMEX")]
        EstrangeiraImportacaoDiretaSemSimilarNacional = 6,

        /// <summary>
        /// 7 - Estrangeira - Adquirida no mercado interno, sem similar nacional, constante em lista da CAMEX e gás natural.
        /// </summary>
        [XmlEnum(Name = "7")]
        [Subtitle("EstrangeiraAdquiridaNoMercadoInterno", "7 - Estrangeira, adquirida no mercado interno sem similar nacional, consta lista CAMEX")]
        EstrangeiraAdquiridaNoMercadoInterno = 7,

        /// <summary>
        /// 8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%;
        /// </summary>
        [XmlEnum(Name = "8")]
        [Subtitle("NacionalConteudoImportacaoSuperior70", "7 - Nacional, conteúdo importação superior a 70%")]
        NacionalConteudoImportacaoSuperior70 = 8
    }
}
