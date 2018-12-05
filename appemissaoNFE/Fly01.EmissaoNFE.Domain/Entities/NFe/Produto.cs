using Fly01.Core.Entities.Domains.Enum;
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
        /// (antigos códigos EAN, UPC e DUN-14), se não possuem código de barras com GTIN, deve ser informado o literal “SEM GTIN”
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
        public int? CFOP { get; set; }

        [Required]
        [MaxLength(6)]
        /// <summary>
        /// informar a unidade de comercialização do produto (Ex. pc, und, dz, kg, etc.).
        /// </summary>
        [XmlElement(ElementName = "uCom")]
        public string UnidadeMedida { get; set; }

        [Required]
        [XmlIgnore]
        /// <summary>
        /// informar a quantidade de comercialização do produto já formatado com ponto decimal. A quantidade de casas decimais pode variar de 0 a 4.
        /// </summary>
        public double Quantidade { get; set; }

        [XmlElement(ElementName = "qCom")]
        public string QuantidadeString
        {
            get { return Quantidade.ToString("0.00").Replace(",", "."); }
            set { Quantidade = double.Parse(value.Replace(".", ",")); }
        }

        [Required]
        [XmlIgnore]
        /// <summary>
        /// Informar o valor unitário de comercialização do produto já formatado com ponto decimal, campo meramente informativo, o contribuinte pode utilizar a precisão 
        /// desejada (0-10 decimais). Para efeitos de cálculo, o valor unitário será obtido pela divisão do valor do produto pela quantidade comercial.
        /// </summary>
        public double ValorUnitario { get; set; }

        [XmlElement(ElementName = "vUnCom")]
        public string ValorUnitarioString
        {
            get { return ValorUnitario.ToString("0.00").Replace(",", "."); }
            set { ValorUnitario = double.Parse(value.Replace(".", ",")); }
        }

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
        /// informar o GTIN (Global Trade Item Number) do produto, antigo código EAN ou código de barras. Preencher com o código GTIN-8, GTIN-12, GTIN-13 ou GTIN-14 
        /// (antigos códigos EAN, UPC e DUN-14), se não possuem código de barras com GTIN, deve ser informado o literal “SEM GTIN”
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
        [XmlIgnore]
        /// <summary>
        /// informar a quantidade de tributação do produto já formatado com ponto decimal. A quantidade de casas decimais pode variar de 0 a 4.
        /// </summary>
        public double QuantidadeTributada { get; set; }

        [XmlElement(ElementName = "qTrib")]
        public string QuantidadeTributadaString
        {
            get { return QuantidadeTributada.ToString("0.00").Replace(",", "."); }
            set { QuantidadeTributada = double.Parse(value.Replace(".", ",")); }
        }

        [Required]
        [XmlIgnore]
        /// <summary>
        /// Informar o valor unitário de tributação do produto já formatado com ponto decimal, campo meramente informativo, o contribuinte pode utilizar a precisão desejada
        /// (0-10 decimais). Para efeitos de cálculo, o valor unitário será obtido pela divisão do valor do produto pela quantidade tributável.
        /// </summary>
        public double ValorUnitarioTributado { get; set; }

        [XmlElement(ElementName = "vUnTrib")]
        public string ValorUnitarioTributadoString
        {
            get { return ValorUnitarioTributado.ToString("0.00").Replace(",", "."); }
            set { ValorUnitarioTributado = double.Parse(value.Replace(".", ",")); }
        }

        [XmlIgnore]
        public double? ValorFrete { get; set; }

        [XmlElement(ElementName = "vFrete", IsNullable = true)]
        public string ValorFreteString
        {
            get { return ValorFrete.HasValue ? ValorFrete.Value.ToString("0.00").Replace(",", ".") : "0.00"; }
            set { ValorFrete = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorFreteString()
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
        [XmlIgnore]
        public double? ValorDesconto { get; set; }

        [XmlElement(ElementName = "vDesc")]
        public string ValorDescontoString
        {
            get { return ValorDesconto.HasValue ? ValorDesconto.Value.ToString("0.00").Replace(",", ".") : null; }
            set { ValorDesconto = double.Parse(value.Replace(".", ",")); }
        }

        public bool ShouldSerializeValorDescontoString()
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

        [XmlIgnore]
        public TipoProduto TipoProduto { get; set; }
    }
}
