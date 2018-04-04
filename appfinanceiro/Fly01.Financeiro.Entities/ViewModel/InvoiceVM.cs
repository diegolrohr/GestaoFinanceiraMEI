using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class InvoiceVM : DomainBaseVM
	{
		//Informa o tipo do Documento de Saída que pode ser: 1=Normal, 2=Devolução, 3=Complemento de Preço, 4=Beneficiamento, 6=Complemento de ICMS, 7=Complemento de IPI
		[JsonProperty("type")]
		[Display(Name = "Tipo")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Type { get; set; }

		//Número da Nota Fiscal
		[JsonProperty("documentNumber")]
		[Display(Name = "Documento")]
		[StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string DocumentNumber { get; set; }

		//Identifica a série da Nota Fiscal
		[JsonProperty("serie")]
		[Display(Name = "Serie")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Serie { get; set; }

		//Status da Nota Fiscal.
		[JsonProperty("status")]
		[Display(Name = "Status")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Status { get; set; }

		//Status da Nota Fiscal.
		[JsonProperty("statusDescription")]
		[Display(Name = "Status")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StatusDescription { get; set; }

		//
		[JsonProperty("nfeSentDescription")]
        [Display(Name = "NfeSentDescription")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string NfeSentDescription { get; set; }

		//Codigo do cliente da Nota Fiscal
		[JsonProperty("personId")]
		[Display(Name = "Pessoa")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PersonId { get; set; }

		//
		[JsonProperty("state")]
		[Display(Name = "Estado")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string State { get; set; }

		//
		[JsonProperty("personName")]
        [Display(Name = "PersonName")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PersonName { get; set; }

		//Data de emissão da Nota Fiscal
		[Display(Name = "Dt.Emissao")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime CreatedAt { get; set; }

        [JsonProperty("createdAt")]
        public string CreatedAtRest
        {
            get
            {
                //return CreatedAt.HasValue ? CreatedAt.Value.ToString("yyyyMMdd") : null;
                return CreatedAt.ToString("yyyyMMdd");
            }
            set
            {
                CreatedAt = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Data de emissão da Nota Fiscal
		[Display(Name = "Dt.Emissao")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime IssueDate { get; set; }

        [JsonProperty("issueDate")]
        public string IssueDateRest
        {
            get
            {
                return IssueDate.ToString("yyyyMMdd");
            }
            set
            {
                IssueDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Espécie do Documento Fiscal.
		[JsonProperty("classId")]
		[Display(Name = "Especie")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ClassId { get; set; }

		//Informe a tabela de preços a ser considerada nos produtos da Nota Fiscal.
		[JsonProperty("priceTableId")]
		[Display(Name = "Tab.Precos")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PriceTableId { get; set; }

		//Descrição da tabela de preços. Apenas informativo.
		[JsonProperty("priceTableDescription")]
		[Display(Name = "Descrição")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string PriceTableDescription { get; set; }

		//Código de identificação da transportadora.
		[JsonProperty("carrierId")]
		[Display(Name = "Transport.")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CarrierId { get; set; }

		//Quantidade de volumes.
		[JsonProperty("parcelQuantity")]
		[Display(Name = "Qtd.Volumes")]
		public int? ParcelQuantity { get; set; }

		//Espécie do volume.
		[JsonProperty("parcelType")]
		[Display(Name = "Especie Vol.")]
		[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ParcelType { get; set; }

		//Marca da mercadoria.
		[JsonProperty("stamp")]
		[Display(Name = "Marca")]
		[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Stamp { get; set; }

		//Número do pedido do cliente.
		[JsonProperty("customerOrder")]
		[Display(Name = "Ped.Cliente")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CustomerOrder { get; set; }

		//Numeração dos volumes transportados
		[JsonProperty("numeration")]
		[Display(Name = "Numeração")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Numeration { get; set; }

		//Peso bruto da mercadoria impresso na nota fiscal.
		[JsonProperty("rawWeight")]
		[Display(Name = "Peso Bruto")]
		public double? RawWeight { get; set; }

		//Peso líquido da mercadoria impresso na nota fiscal.
		[JsonProperty("netWeight")]
		[Display(Name = "Peso Liquido")]
		public double? NetWeight { get; set; }

		//Informe o tipo de frete do movimento: CIF é quando o remetente manda o frete pago e FOB é quando quem paga o frete é o destinatário.
		[JsonProperty("shipType")]
		[Display(Name = "Tipo Frete")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ShipType { get; set; }

		//Numero da Nota Fiscal Eletronica
		[JsonProperty("numberEletronicInvoice")]
		[Display(Name = "Nf Eletr.")]
		[StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string NumberEletronicInvoice { get; set; }

		//Data de emissão da Nota Fiscal Eletronica
		[Display(Name = "Emissão Nf-E")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? IssueEletronicInvoice { get; set; }

        [JsonProperty("issueEletronicInvoice")]
        public string IssueEletronicInvoiceRest
        {
            get
            {
                return IssueEletronicInvoice.HasValue ? IssueEletronicInvoice.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                IssueEletronicInvoice = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Horario de emissão da Nota Fiscal Eletronica
		[JsonProperty("hourEletronicInvoice")]
		[Display(Name = "Hora Nf-E")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string HourEletronicInvoice { get; set; }

		//Valor do crédito concedido na Nota Fiscal Eletrônica
		[JsonProperty("loansGrantedElectronicInvoice")]
		[Display(Name = "Cred. Conced")]
		public double? LoansGrantedElectronicInvoice { get; set; }

		//Nota + Série que utilizou este documento como base de calculo do PIS/COFINS/CSLL
		[JsonProperty("baseTaxes")]
		[Display(Name = "Base Pcc")]
		[StringLength(12, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string BaseTaxes { get; set; }

		//Nota+ Série que utilizou este documento como base de calculo do IRRF
		[JsonProperty("valueIncomeTax")]
		[Display(Name = "Nf Valor Irrf")]
		[StringLength(12, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ValueIncomeTax { get; set; }

		//Percentual de comissão sobre o cliente neste documento de saída.
		[JsonProperty("customerComissionRate")]
		[Display(Name = "%Comiss.Cli.")]
		public double? CustomerComissionRate { get; set; }

		//Data de saida do Documento de Saída
		[Display(Name = "Data")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? OutputDate { get; set; }

        [JsonProperty("outputDate")]
        public string OutputDateRest
        {
            get
            {
                return OutputDate.HasValue ? OutputDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                OutputDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Horario de saida do Documento de Saída
		[JsonProperty("outputHour")]
		[Display(Name = "Hora")]
		[StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string OutputHour { get; set; }

		//Armazena o código do Município para prestação de serviços
		[JsonProperty("cityProvided")]
		[Display(Name = "Mun.Prest.")]
		[StringLength(7, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CityProvided { get; set; }

		//Armazena a descrição do Município para prestação de serviços
		[JsonProperty("cityName")]
		[Display(Name = "Desc.Munic.")]
		[StringLength(35, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CityName { get; set; }

		//Informe a UF de embarque
		[JsonProperty("stateBoarding")]
		[Display(Name = "Uf Emb")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StateBoarding { get; set; }

		//Informe o local de embarque
		[JsonProperty("boardingLocation")]
		[Display(Name = "Loc Emb")]
		[StringLength(60, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string BoardingLocation { get; set; }

		//Código de validação da Nota Fiscal Eletrônica
		[JsonProperty("verificationCodeEletronicInvoice")]
		[Display(Name = "Cd.Ver. Nf-E")]
		[StringLength(24, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string VerificationCodeEletronicInvoice { get; set; }

		//Numero da chave de acesso da NFe
		[JsonProperty("eletronicKeyInvoice")]
		[Display(Name = "Chave Nfe")]
		[StringLength(44, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string EletronicKeyInvoice { get; set; }

		//Endereço alternativo de Entrega
		[JsonProperty("alternativAddressId")]
		[Display(Name = "Endereço Alt.")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string AlternativAddressId { get; set; }

		//Data de Autorizacao da NF
		[Display(Name = "Data De Auto")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
		public DateTime? AuthorizationDate { get; set; }

        [JsonProperty("authorizationDate")]
        public string AuthorizationDateRest
        {
            get
            {
                return AuthorizationDate.HasValue ? AuthorizationDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                AuthorizationDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

		//Hora de Autorizacao da NF
		[JsonProperty("authorizationHour")]
		[Display(Name = "Hora De Auto")]
		[StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string AuthorizationHour { get; set; }

		//
		[JsonProperty("workCode")]
		[Display(Name = "Código Da Obra")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string WorkCode { get; set; }

		//
		[JsonProperty("ARTCode")]
		[Display(Name = "Código Art")]
		[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ARTCode { get; set; }

		//Indicador de presença do comprador no estabelecimento comercial no momento da operação.
		[JsonProperty("buyerPresent")]
		[Display(Name = "Presenca Com.")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string BuyerPresent { get; set; }

		//Horário de emissão do documento
		[JsonProperty("issueTime")]
		[Display(Name = "Hora Emissão")]
		[StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string IssueTime { get; set; }

		//
		[JsonProperty("coupon")]
		[Display(Name = "Cupom")]
		[StringLength(21, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Coupon { get; set; }

		//Indica se recolhe ou não o ISS para NFSE
		[JsonProperty("collectISS")]
		[Display(Name = "Recolhe Iss?")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CollectISS { get; set; }

		//Código do vendedor principal
		[JsonProperty("seller1Id")]
		[Display(Name = "Vendedor")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Seller1Id { get; set; }

		//Comissão do vendedor principal
		[JsonProperty("seller1ComissionRate")]
		[Display(Name = "% Comis.")]
		public double? Seller1ComissionRate { get; set; }

		//Percentual da comissão do primeiro vendedor a ser pago na emissão do documento de saída.
		[JsonProperty("seller1EmissionRate")]
		[Display(Name = "% Emissão")]
		public double? Seller1EmissionRate { get; set; }

		//Percentual da comissão do primeiro vendedor a ser pago de acordo com o recebimento dos lançamentos financeiros (A Receber), gerados de acordo com a condição de pagamento.
		[JsonProperty("seller1IssueRate")]
		[Display(Name = "% Baixa")]
		public double? Seller1IssueRate { get; set; }

		//Código do segundo vendedor
		[JsonProperty("seller2Id")]
		[Display(Name = "Vendedor 2")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Seller2Id { get; set; }

		//Comissão do segundo vendedor
		[JsonProperty("seller2ComissionRate")]
		[Display(Name = "% Comis. 2")]
		public double? Seller2ComissionRate { get; set; }

		//Percentual da comissão do segundo vendedor a ser pago na emissão do documento de saída.
		[JsonProperty("seller2EmissionRate")]
		[Display(Name = "% Emissão 2")]
		public double? Seller2EmissionRate { get; set; }

		//Percentual da comissão do segundo vendedor a ser pago de acordo com o recebimento dos lançamentos financeiros (A Receber), gerados de acordo com a condição de pagamento.
		[JsonProperty("seller2IssueRate")]
		[Display(Name = "% Baixa 2")]
		public double? Seller2IssueRate { get; set; }

		//Código do terceiro vendedor para esse faturamento.
		[JsonProperty("seller3Id")]
		[Display(Name = "Vendedor 3")]
		[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string Seller3Id { get; set; }

		//Percentual de comissão para o terceiro  vendedor.
		[JsonProperty("seller3ComissionRate")]
		[Display(Name = "% Comis. 3")]
		public double? Seller3ComissionRate { get; set; }

		//Percentual da comissão do terceiro vendedor a ser pago na emissão do documento de saída.
		[JsonProperty("seller3EmissionRate")]
		[Display(Name = "% Emissão 3")]
		public double? Seller3EmissionRate { get; set; }

		//Percentual da comissão do terceiro  vendedor a ser pago de acordo com o recebimento dos lançamentos financeiros (A Receber), gerados de acordo com a condição de pagamento.
		[JsonProperty("seller3IssueRate")]
		[Display(Name = "% Baixa 3")]
		public double? Seller3IssueRate { get; set; }

		//Mensagem da Nota Fiscal
		[JsonProperty("invoiceMessage")]
		[Display(Name = "Mens.Nota")]
		public string InvoiceMessage { get; set; }

		//Código da mensagem impressa na nota fiscal. Mensagem Padrão cadastrada atraves de uma Formula.
		[JsonProperty("standardMessage1Id")]
		[Display(Name = "Men Pad 1")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StandardMessage1Id { get; set; }

		//Código da mensagem impressa na nota fiscal. Mensagem Padrão cadastrada atraves de uma Formula.
		[JsonProperty("standardMessage2Id")]
		[Display(Name = "Men Pad 2")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StandardMessage2Id { get; set; }

		//Código da mensagem impressa na nota fiscal. Mensagem Padrão cadastrada atraves de uma Formula.
		[JsonProperty("standardMessage3Id")]
		[Display(Name = "Men Pad 3")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StandardMessage3Id { get; set; }

		//Código da mensagem impressa na nota fiscal. Mensagem Padrão cadastrada atraves de uma Formula.
		[JsonProperty("standardMessage4Id")]
		[Display(Name = "Men Pad 4")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StandardMessage4Id { get; set; }

		//Código da mensagem impressa na nota fiscal. Mensagem Padrão cadastrada atraves de uma Formula.
		[JsonProperty("standardMessage5Id")]
		[Display(Name = "Men Pad 5")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string StandardMessage5Id { get; set; }

		//Código do veículo cadastrado na ANTT(Agência Nacional de Transportes Terrestres).
		[JsonProperty("ANTTcode")]
		[Display(Name = "Cod.Antt")]
		[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ANTTcode { get; set; }

		//Placa do Veículo.
		[JsonProperty("carPlate")]
		[Display(Name = "Placa")]
		[StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CarPlate { get; set; }

		//Estado Federativo do Veículo.
		[JsonProperty("carState")]
		[Display(Name = "Uf Placa")]
		[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CarState { get; set; }

        //
        [JsonProperty("paymentFormId")]
        [Display(Name = "Forma Pagto")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string PaymentFormId { get; set; }

        //
		[JsonProperty("conditionId")]
		[Display(Name = "Cond.Pagto.")]
		[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ConditionId { get; set; }

		//
		[JsonProperty("dueDate")]
		[Display(Name = "DueDate")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string DueDate { get; set; }

		//Informe o codigo da categoria
		[JsonProperty("categoryId")]
		[Display(Name = "Categoria")]
		[Required(ErrorMessage = "O campo {0} é obrigatório.")]
		[StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CategoryId { get; set; }

		//Informe a moeda do título
		[JsonProperty("currencyId")]
		[Display(Name = "Moeda")]
		[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CurrencyId { get; set; }

		//Chave de Acesso da NF-e recebida para exportação
		[JsonProperty("exportKeyInvoice")]
		[Display(Name = "Chave De Acesso De Exportação")]
		[StringLength(44, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string ExportKeyInvoice { get; set; }

		//
		[JsonProperty("totalValueProducts")]
		[Display(Name = "Vlr.Mercadoria")]
		public double? TotalValueProducts { get; set; }

		//
		[JsonProperty("totalValueFreight")]
		[Display(Name = "Vlr.Frete")]
		public double? TotalValueFreight { get; set; }

		//
		[JsonProperty("totalSpending")]
		[Display(Name = "Vlr.Despesas")]
		public double? TotalSpending { get; set; }

		//
		[JsonProperty("totalTaxes")]
		[Display(Name = "Total Taxas")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string TotalTaxes { get; set; }

		//
		[JsonProperty("totalInvoice")]
		[Display(Name = "Vlr.Total")]
		public double? TotalInvoice { get; set; }

		//Valor total do desconto concedido.
		[JsonProperty("totalDiscount")]
		[Display(Name = "Vlr.Desconto")]
		public double? TotalDiscount { get; set; }

		//
		[JsonProperty("totalValueFreight+totalSpending")]
        [Display(Name = "TotalValueFreight+totalSpending")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string TotalValueFreightTotalSpending { get; set; }

		//
		[JsonProperty("totalBills")]
		[Display(Name = "Base Das Duplicatas")]
		public double? TotalBills { get; set; }

		//
		[JsonProperty("totalIPI")]
		[Display(Name = "Vlr.Do Ipi")]
		public double? TotalIPI { get; set; }

		//
		[JsonProperty("totalICMS")]
		[Display(Name = "Vlr.Do Icms")]
		public double? TotalICMS { get; set; }

		//
		[JsonProperty("totalICMSST")]
		[Display(Name = "Vlr.Icms Solid.")]
		public double? TotalICMSST { get; set; }

		//
		[JsonProperty("totalPIS")]
		[Display(Name = "Vlr.Do Pis")]
		public double? TotalPIS { get; set; }

		//
		[JsonProperty("totalCOFINS")]
		[Display(Name = "Vlr.Do Cofins")]
		public double? TotalCOFINS { get; set; }

		//
		[JsonProperty("subtitleCode")]
        [Display(Name = "SubtitleCode")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string SubtitleCode { get; set; }

		//
		[JsonProperty("canSendNFe")]
        [Display(Name = "CanSendNFe")]
		[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
		public string CanSendNFe { get; set; }

        //
        [JsonProperty("invoiceItems")]
        [Display(Name = "InvoiceItems")]
        public List<InvoiceItemsVM> InvoiceItems { get; set; }

	}
}
