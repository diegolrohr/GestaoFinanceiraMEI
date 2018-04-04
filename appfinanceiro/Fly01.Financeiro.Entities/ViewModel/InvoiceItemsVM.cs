using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class InvoiceItemsVM : DomainBaseVM
	{
		//
		[JsonProperty("invoiceId")]
        [Display(Name = "InvoiceId")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string InvoiceId { get; set; }

		//
		[JsonProperty("documentNumber")]
		[Display(Name = "Documento")]
		[StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string DocumentNumber { get; set; }

		//
		[JsonProperty("personId")]
		[Display(Name = "Pessoa")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PersonId { get; set; }

		//
		[JsonProperty("serie")]
		[Display(Name = "Serie")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Serie { get; set; }

		//
		[JsonProperty("type")]
		[Display(Name = "Tipo")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Type { get; set; }

		//
		[JsonProperty("item")]
		[Display(Name = "Item")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Item { get; set; }

		//Informe o código do produto.
		[JsonProperty("productId")]
		[Display(Name = "Produto")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ProductId { get; set; }

		//Descrição do produto a ser emitido na nota. É apresentado como default o nome que está no cadastro de produto.
		[JsonProperty("productDescription")]
		[Display(Name = "Descricao")]
		[StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ProductDescription { get; set; }

		//
		[JsonProperty("measureId")]
		[Display(Name = "Unid Medida")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string MeasureId { get; set; }

		//Segunda Unidade de Medida utilizada para se referir ao Produto.
		[JsonProperty("measure2Id")]
		[Display(Name = "Seg.Un.Med")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Measure2Id { get; set; }

		//
		[JsonProperty("balanceStock")]
		[Display(Name = "Saldo Em Estoque")]
		public double? BalanceStock { get; set; }

		//Quantidade do produto no documento de saida
		[JsonProperty("quantity")]
		[Display(Name = "Quantidade")]
		public double? Quantity { get; set; }

		//Quantidade original da solicitação na unidade secundária.
		[JsonProperty("quantity2Id")]
		[Display(Name = "Qtd.Seg.Un.")]
		public double? Quantity2Id { get; set; }

		//Preço unitário do produto no documento de saída
		[JsonProperty("unitPrice")]
		[Display(Name = "Preco Unit.")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public double UnitPrice { get; set; }

		//Preço total do item no documento de saída
		[JsonProperty("amount")]
		[Display(Name = "Valor Total")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		public double Amount { get; set; }

		//Informe o código da categoria financeira.
		[JsonProperty("categoryId")]
		[Display(Name = "Cod.Categoria")]
		[StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CategoryId { get; set; }

		//Valor do IPI
		[JsonProperty("IPIValue")]
		[Display(Name = "Vlr. Ipi")]
		public double? IPIValue { get; set; }

		//Valor do ICM.
		[JsonProperty("ICMSValue")]
		[Display(Name = "Vlr. Icms")]
		public double? ICMSValue { get; set; }

		//Código do tipo de operação ou movimentação do material.
		[JsonProperty("operationType")]
		[Display(Name = "Tp. Operação")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OperationType { get; set; }

		//Código do TES (Tipo de Entrada e Saída).
        //A tes define se o item movimenta estoque, se é somado no total a receber e como os impostos são calculados

		[JsonProperty("outflowType")]
		[Display(Name = "Tes")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OutflowType { get; set; }

		//Código da CFOP
		[JsonProperty("cfop")]
		[Display(Name = "Cfop")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Cfop { get; set; }

		//Percentual de Desconto do Item.
		[JsonProperty("discountRate")]
		[Display(Name = "% Desc.")]
		public double? DiscountRate { get; set; }

		//Valor de desconto do item
		[JsonProperty("discountValue")]
		[Display(Name = "Vlr. Desconto")]
		public double? DiscountValue { get; set; }

		//Aliquota de IPI do Produto.
		[JsonProperty("IPIRate")]
		[Display(Name = "Aliq. Ipi")]
		public double? IPIRate { get; set; }

		//Aliquota de ICMS do Produto.
		[JsonProperty("ICMSRate")]
		[Display(Name = "Aliq.Icms")]
		public double? ICMSRate { get; set; }

		//Valor base do IPI pra este item
		[JsonProperty("IPIBase")]
		[Display(Name = "Base Ipi")]
		public double? IPIBase { get; set; }

		//Valor base do ICMS pra este item
		[JsonProperty("ICMSBase")]
		[Display(Name = "Base Icms")]
		public double? ICMSBase { get; set; }

		//Armazém de origem
		[JsonProperty("warehouseId")]
		[Display(Name = "Armazem")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string WarehouseId { get; set; }

		//
		[JsonProperty("orderItemId")]
        [Display(Name = "OrderItemId")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OrderItemId { get; set; }

		//
		[JsonProperty("orderId")]
		[Display(Name = "Pedido")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OrderId { get; set; }

		//
		[JsonProperty("orderItem")]
		[Display(Name = "Item Pv")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OrderItem { get; set; }

		//Valor do ICMS retido do item.
		[JsonProperty("ICMSSolidarity")]
		[Display(Name = "Icms Solid.")]
		public double? ICMSSolidarity { get; set; }

		//Valor base do ICMS Retido (Solidário)
		[JsonProperty("ICMSSolidarityBase")]
		[Display(Name = "Bs.Icms Solid.")]
		public double? ICMSSolidarityBase { get; set; }

		//Base de ISS do produto
		[JsonProperty("ISSBase")]
		[Display(Name = "Base Iss")]
		public double? ISSBase { get; set; }

		//Aliquota de ISS do produto
		[JsonProperty("ISSRate")]
		[Display(Name = "Aliq.Iss")]
		public double? ISSRate { get; set; }

		//Aliquota de IRRF do produto
		[JsonProperty("IRRFRate")]
		[Display(Name = "Aliquota Irrf")]
		public double? IRRFRate { get; set; }

		//Valor de ISS do produto
		[JsonProperty("ISSValue")]
		[Display(Name = "Valor Iss")]
		public double? ISSValue { get; set; }

		//Base base de INSS
		[JsonProperty("INSSBase")]
		[Display(Name = "Base Inss")]
		public double? INSSBase { get; set; }

		//Valor de INSS para este item
		[JsonProperty("INSSValue")]
		[Display(Name = "Valor Inss")]
		public double? INSSValue { get; set; }

		//Valor do item referente ao complemento de preço
		[JsonProperty("ICMSAdditionalValue")]
		[Display(Name = "Vlr.Icms Comp.")]
		public double? ICMSAdditionalValue { get; set; }

		//
		[JsonProperty("originalInvoiceItemId")]
        [Display(Name = "OriginalInvoiceItemId")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OriginalInvoiceItemId { get; set; }

		//Numero da Nota Fiscal original.
		[JsonProperty("originalInvoiceDocumentNumber")]
		[Display(Name = "Doc.Origem")]
		[StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OriginalInvoiceDocumentNumber { get; set; }

		//Serie da Nota Fiscal original.
		[JsonProperty("originalInvoiceSerie")]
		[Display(Name = "Serie Origem")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OriginalInvoiceSerie { get; set; }

		//Numero do item da Nota Fiscal original.
		[JsonProperty("originalInvoiceItem")]
		[Display(Name = "Item Origem")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OriginalInvoiceItem { get; set; }

		//Custo médio do movimento na 1a moeda.
		[JsonProperty("realCost")]
		[Display(Name = "Cst. Real")]
		public double? RealCost { get; set; }

		//Custo médio do movimento na 2a moeda.
		[JsonProperty("dolarCost")]
		[Display(Name = "Cst. Dolar")]
		public double? DolarCost { get; set; }

		//Custo médio do movimento na 3a moeda.
		[JsonProperty("pesoCost")]
		[Display(Name = "Cst. Peso")]
		public double? PesoCost { get; set; }

		//Custo médio do movimento na 4a moeda.
		[JsonProperty("euroCost")]
		[Display(Name = "Cst. Euro")]
		public double? EuroCost { get; set; }

		//Custo médio do movimento na 5a moeda.
		[JsonProperty("ieneCost")]
		[Display(Name = "Cst. Iene")]
		public double? IeneCost { get; set; }

		//Código da situação tributária.
		[JsonProperty("fiscalClass")]
		[Display(Name = "Sit.Tribut.")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string FiscalClass { get; set; }

		//Aliquota do PIS
		[JsonProperty("PISValue")]
		[Display(Name = "Valor Do Pis")]
		public double? PISValue { get; set; }

		//Valor da base do PIS
		[JsonProperty("PISBase")]
		[Display(Name = "Base Pis")]
		public double? PISBase { get; set; }

		//Valor da aliquota de PIS
		[JsonProperty("PISRate")]
		[Display(Name = "Aliquota Pis")]
		public double? PISRate { get; set; }

		//Valor do COFINS
		[JsonProperty("COFINSValue")]
		[Display(Name = "Valor Do Cofins")]
		public double? COFINSValue { get; set; }

		//Valor da base de COFINS
		[JsonProperty("COFINSBase")]
		[Display(Name = "Base Cofins")]
		public double? COFINSBase { get; set; }

		//Aliquota do COFINS
		[JsonProperty("COFINSRate")]
		[Display(Name = "Aliquota Cofins")]
		public double? COFINSRate { get; set; }

		//Valor de CSLL
		[JsonProperty("CSLLValue")]
		[Display(Name = "Valor Do Csll")]
		public double? CSLLValue { get; set; }

		//Valor da base de CSLL
		[JsonProperty("CSLLBase")]
		[Display(Name = "Base Csll")]
		public double? CSLLBase { get; set; }

		//Valor da base de IRRF
		[JsonProperty("IRRFBase")]
		[Display(Name = "Vlr.Base Irrf")]
		public double? IRRFBase { get; set; }

		//Valor de IRRF para este item
		[JsonProperty("IRRFValue")]
		[Display(Name = "Vlr.Irrf")]
		public double? IRRFValue { get; set; }

		//
		[JsonProperty("ident3Party")]
		[Display(Name = "Ident.Poder3")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Ident3Party { get; set; }

		//Código do lote do produto.
		[JsonProperty("lot")]
		[Display(Name = "Lote")]
		[StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Lot { get; set; }

		//Data de Fabricação do Lote de Produtos.		
		[Display(Name = "Dt. Fabricação")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? ManufacturingDate { get; set; }

        [JsonProperty("manufacturingDate")]
        public string ManufacturingDateRest
        {
            get
            {
                return ManufacturingDate.HasValue ? ManufacturingDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                ManufacturingDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Data de validade do lote.		
		[Display(Name = "Validade")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? ExpirationDate { get; set; }

        [JsonProperty("expirationDate")]
        public string ExpirationDateRest
        {
            get
            {
                return ExpirationDate.HasValue ? ExpirationDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                ExpirationDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//
		[JsonProperty("comissionRate")]
		[Display(Name = "%Comissao")]
		public double? ComissionRate { get; set; }

		//
		[JsonProperty("comissionRuleRate")]
		[Display(Name = "%Regra Comiss.")]
		public double? ComissionRuleRate { get; set; }

		//
		[JsonProperty("ICMSNBase")]
		[Display(Name = "Base Icmsn")]
		public double? ICMSNBase { get; set; }

		//
		[JsonProperty("ICMSNValue")]
		[Display(Name = "Val Icmsn")]
		public double? ICMSNValue { get; set; }

		//
		[JsonProperty("csosn")]
		[Display(Name = "Csosn")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Csosn { get; set; }

		//Numero do Pedido de Compra
		[JsonProperty("purchaseOrderNumber")]
		[Display(Name = "Numero Do Pedido De Compra")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PurchaseOrderNumber { get; set; }

		//Item do Pedido de Compra
		[JsonProperty("purchaseOrderItem")]
		[Display(Name = "Item Do Pedido De Compra")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PurchaseOrderItem { get; set; }

		//
		[JsonProperty("ICMSdifferentiatedValue")]
		[Display(Name = "Vlr.Icms Dif")]
		public double? ICMSdifferentiatedValue { get; set; }

		//Código da FCI, retornado após validação do FCI, segundo especificações do SINIEF nº 19/2012.
		[JsonProperty("FCICode")]
		[Display(Name = "Código Fci")]
		[StringLength(36, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string FCICode { get; set; }

		//Drawback para exportação
		[JsonProperty("Drawback")]
		[Display(Name = "Drawback")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Drawback { get; set; }

		//
		[JsonProperty("priceTable")]
		[Display(Name = "Tab.Precos")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PriceTable { get; set; }

		//Chave de Acesso da NF-e recebida para exportação
		[JsonProperty("exportKeyInvoice")]
		[Display(Name = "Chave De Acesso De Exportação")]
		[StringLength(44, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ExportKeyInvoice { get; set; }

		//Número do Registro de Exportação
		[JsonProperty("exportRegistration")]
		[Display(Name = "Registro De Exportação")]
		[StringLength(12, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ExportRegistration { get; set; }

		//Quantidade do item efetivamente exportado
		[JsonProperty("exportQuantity")]
		[Display(Name = "Quantidade Exportada")]
		public double? ExportQuantity { get; set; }

	}
}
