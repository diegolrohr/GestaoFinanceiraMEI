using Fly01.EmissaoNFE.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Fly01.EmissaoNFE.Domain.Entities.NFe
{
    public class Produto
    {
        [Required]
        [MaxLength(60)]
        /// <summary>
        /// informar o código do produto ou serviço. Preencher com CFOP, caso se trate de itens não relacionados 
        /// com mercadorias/produtos e que o contribuinte não possua codificação própria. Formato ”CFOP9999”.
        /// </summary>
        [XmlElement(ElementName = "cProd")]
        public string Codigo { get; set; }

        [Required]
        [MaxLength(14)]
        /// <summary>
        /// informar o GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras. Preencher com o código GTIN-8, GTIN-12, GTIN-13 ou GTIN-14 
        /// (antigos códigos EAN, UPC e DUN-14), não informar o conteúdo da TAG em caso de o produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEAN")]
        public string GTIN { get; set; }

        [Required]
        [MaxLength(120)]
        /// <summary>
        /// informar a descrição do produto ou serviço.
        /// </summary>
        [XmlElement(ElementName = "xProd")]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(8)]
        /// <summary>
        /// informar o Código NCM com 8 dígitos; informar a posição do capítulo do NCM (as duas primeiras posições do NCM) quando a operação não for de comércio exterior
        /// (importação/ exportação) ou o produto não seja tributado pelo IPI; se for serviços, informar 00.
        /// </summary>
        [XmlElement(ElementName = "NCM")]
        public string NCM { get; set; }
        
        [XmlElement(ElementName = "CEST")]
        public string CEST { get; set; }

        [MaxLength(3)]
        /// <summary>
        /// informar de acordo com o código EX da TIPI se houver para o NCM do produto.
        /// </summary>
        [XmlElement(ElementName = "EXTIPI")]
        public string EXTIPI { get; set; }

        [Required]
        /// <summary>
        /// informar o CFOP - Código Fiscal de Operações e Prestações.
        /// </summary>
        [XmlElement(ElementName = "CFOP")]
        public int CFOP { get; set; }

        [Required]
        [MaxLength(6)]
        /// <summary>
        /// informar a unidade de comercialização do produto (Ex. pc, und, dz, kg, etc.).
        /// </summary>
        [XmlElement(ElementName = "uCom")]
        public string UnidadeMedida { get; set; }

        [Required]
        [MaxLength(20)]
        /// <summary>
        /// informar a quantidade de comercialização do produto já formatado com ponto decimal. A quantidade de casas decimais pode variar de 0 a 4.
        /// </summary>
        [XmlElement(ElementName = "qCom")]
        public string Quantidade { get; set; }

        [Required]
        /// <summary>
        /// Informar o valor unitário de comercialização do produto já formatado com ponto decimal, campo meramente informativo, o contribuinte pode utilizar a precisão 
        /// desejada (0-10 decimais). Para efeitos de cálculo, o valor unitário será obtido pela divisão do valor do produto pela quantidade comercial.
        /// </summary>
        [XmlElement(ElementName = "vUnCom")]
        public double ValorUnitario { get; set; }

        [Required]
        /// <summary>
        /// informar o valor total bruto do produto ou serviços.
        /// </summary>
        [XmlIgnore]
        public double ValorBruto { get; set; }

        [XmlElement(ElementName = "vProd")]
        public string ValorBrutoString
        {
            get { return ValorBruto.ToString("0.00").Replace(",", "."); }
            set { ValorBruto = double.Parse(value.Replace(".", ",")); }
        }

        [Required]
        [MaxLength(14)]
        /// <summary>
        /// informar o GTIN (Global Trade Item Number) da unidade de tributação do produto, antigo código EAN ou código de barras. Preencher com o código GTIN-8, GTIN-12,
        /// GTIN-13 ou GTIN-14 (antigos códigos EAN, UPC e DUN-14), não informar o conteúdo da TAG em caso de o produto não possuir este código.
        /// </summary>
        [XmlElement(ElementName = "cEANTrib")]
        public string GTIN_UnidadeMedidaTributada { get; set; }

        [Required]
        [MaxLength(6)]
        /// <summary>
        /// informar a unidade de tributação do produto (Ex. pc, und, dz, kg, etc.).
        /// </summary>
        [XmlElement(ElementName = "uTrib")]
        public string UnidadeMedidaTributada { get; set; }

        [Required]
        [MaxLength(20)]
        /// <summary>
        /// informar a quantidade de tributação do produto já formatado com ponto decimal. A quantidade de casas decimais pode variar de 0 a 4.
        /// </summary>
        [XmlElement(ElementName = "qTrib")]
        public string QuantidadeTributada { get; set; }

        [Required]
        /// <summary>
        /// Informar o valor unitário de tributação do produto já formatado com ponto decimal, campo meramente informativo, o contribuinte pode utilizar a precisão desejada
        /// (0-10 decimais). Para efeitos de cálculo, o valor unitário será obtido pela divisão do valor do produto pela quantidade tributável.
        /// </summary>
        [XmlElement(ElementName = "vUnTrib")]
        public double ValorUnitarioTributado { get; set; }

        [XmlElement(ElementName = "vFrete", IsNullable = true)]
        public double? ValorFrete { get; set; }

        public bool ShouldSerializeValorFrete()
        {
            return ValorFrete.HasValue & ValorFrete > 0;
        }

        [XmlElement(ElementName = "vSeg", IsNullable = true)]
        public double? ValorSeguro { get; set; }

        public bool ShouldSerializeValorSeguro()
        {
            return ValorSeguro.HasValue & ValorSeguro > 0;
        }

        /// <summary>
        /// Este campo deverá ser preenchido com: 0 - o valor do item (vProd) não compõe o valor total da NF-e (vProd) 1 - o valor do item (vProd) compõe o valor total da NF-e.
        /// </summary>
        [XmlElement(ElementName = "vDesc", IsNullable = true)]
        public double? ValorDesconto { get; set; }

        public bool ShouldSerializeValorDesconto()
        {
            return ValorDesconto.HasValue & ValorDesconto > 0;
        }

        [XmlElement(ElementName = "vOutro", IsNullable = true)]
        public double? ValorOutrasDespesas { get; set; }

        public bool ShouldSerializeValorOutrasDespesas()
        {
            return ValorOutrasDespesas.HasValue & ValorOutrasDespesas > 0;
        }

        [Required]
        [XmlElement(ElementName = "indTot")]
        public CompoemValorTotal AgregaTotalNota { get; set; }
    }
}
