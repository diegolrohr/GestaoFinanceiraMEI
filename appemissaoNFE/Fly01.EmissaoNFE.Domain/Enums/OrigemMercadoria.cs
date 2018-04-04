using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Enums
{
    public enum OrigemMercadoria
    {
        /// <summary>
        /// 0 - Nacional, exceto as indicadas nos códigos 3, 4, 5 e 8;
        /// </summary>
        [XmlEnum(Name = "0")]
        Nacional = 0,
        
        /// <summary>
        /// 1 - Estrangeira - Importação direta, exceto a indicada no código 6;
        /// </summary>
        [XmlEnum(Name = "1")]
        EstrangeiraImportacaoDireta = 1,

        /// <summary>
        /// 2 - Estrangeira - Adquirida no mercado interno, exceto a indicada no código 7;
        /// </summary>
        [XmlEnum(Name = "2")]
        EstrangeiraAdquiridaMercadoInterno = 2,

        /// <summary>
        /// 3 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 40% e inferior ou igual a 70%;
        /// </summary>
        [XmlEnum(Name = "3")]
        NacionalConteudoImportacaoEntre40e70 = 3,

        /// <summary>
        /// 4 - Nacional, cuja produção tenha sido feita em conformidade com os processos produtivos básicos de que tratam as legislações citadas nos Ajustes;
        /// </summary>
        [XmlEnum(Name = "4")]
        NacionalProducaoEmConformidade = 4,

        /// <summary>
        /// 5 - Nacional, mercadoria ou bem com Conteúdo de Importação inferior ou igual a 40%;
        /// </summary>
        [XmlEnum(Name = "5")]
        NacionalConteudoImportacaoInferior40 = 5,

        /// <summary>
        /// 6 - Estrangeira - Importação direta, sem similar nacional, constante em lista da CAMEX e gás natural;
        /// </summary>
        [XmlEnum(Name = "6")]
        EstrangeiraImportacaoDiretaSemSimilarNacional = 6,

        /// <summary>
        /// 7 - Estrangeira - Adquirida no mercado interno, sem similar nacional, constante em lista da CAMEX e gás natural.
        /// </summary>
        [XmlEnum(Name = "7")]
        EstrangeiraAdquiridaNoMercadoInterno = 7,

        /// <summary>
        /// 8 - Nacional, mercadoria ou bem com Conteúdo de Importação superior a 70%;
        /// </summary>
        [XmlEnum(Name = "8")]
        NacionalConteudoImportacaoSuperior70 = 8
    }
}
