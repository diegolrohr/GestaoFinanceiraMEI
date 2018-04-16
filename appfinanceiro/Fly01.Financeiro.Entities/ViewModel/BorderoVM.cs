using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class BorderoVM : DomainBaseVM
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        //
        [JsonProperty("subtitleCode")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SubtitleCode { get; set; }

        [JsonProperty("bankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BancoId { get; set; }

        //
        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CodigoBanco { get; set; }

        //Nome do banco
        [JsonProperty("bankName")]
        [Display(Name = "Nome Banco")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeBanco { get; set; }

        //
        [JsonProperty("bankAgency")]
        [Display(Name = "Nro Agencia")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agencia { get; set; }

        //
        [JsonProperty("bankAccount")]
        [Display(Name = "Nro Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Conta { get; set; }

        //
        [JsonProperty("contract")]
        [Display(Name = "Código do convênio")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Convenio { get; set; }

        //
        [JsonProperty("transactionCode")]
        [Display(Name = "Modelo")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Modelo { get; set; }

        //
        [JsonProperty("historical")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Historico { get; set; }

        //
        [JsonProperty("operatingDate")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DataOperacao { get; set; }

        //
        [JsonProperty("creditValue")]
        [Display(Name = "Val Credito")]
        public double? ValorCredito { get; set; }

        //Informe se o título irá gerar CNAB.
        [JsonProperty("cnabGenerates")]
        [Display(Name = "Gera Cnab")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string GeraCNAB { get; set; }

        //Número do título gerado pelo sistema.
        [JsonProperty("ourNumber")]
        [Display(Name = "Nosso Numero")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NossoNumero { get; set; }

        //Informe a situação do CNAB
        [JsonProperty("CNABsituation")]
        [Display(Name = "Situacao")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SituacaoCNAB { get; set; }

        [JsonProperty("CNABsituationDescription")]
        public string DescricaoSituacaoCNAB { get; set; }

        //Informe a primeira instrução de cobrança
        [JsonProperty("1CNABInstruction")]
        [Display(Name = "1A.Instr.Cnab")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InstrucaoCNAB1 { get; set; }

        [JsonProperty("CNABInstruction1Description")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoInstrucaoCNAB1 { get; set; }

        //Informe a segunda intsrução de cobrança
        [JsonProperty("2CNABInstruction")]
        [Display(Name = "2A.Instr.Cnab")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InstrucaoCNAB2 { get; set; }

        [JsonProperty("CNABInstruction2Description")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoInstrucaoCNAB2 { get; set; }

        //Informe o local da impressão do título.
        [JsonProperty("printingLocation")]
        [Display(Name = "Loc.Impressao")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string LocalImpressao { get; set; }

        //Selecione o tipo da carteria:          
        //Simples sem registro.                   
        //Simples com registro.                   
        //Penhor com registro.
        [JsonProperty("wallet")]
        [Display(Name = "Carteira")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Carteira { get; set; }

        //
        [JsonProperty("walletDescription")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoCarteira { get; set; }

        //Informe a especie do título.
        [JsonProperty("kind")]
        [Display(Name = "Especie")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Especie { get; set; }

        [JsonProperty("kindDescription")]
        [Display(Name = "Especie")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoEspecie { get; set; }

        //Informe o tipo de juros do título
        [JsonProperty("typeOfInterest")]
        [Display(Name = "Tipo Juros")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TipoJuros { get; set; }

        [JsonProperty("discountType")]
        public string TipoDesconto { get; set; }

        [JsonProperty("discountTypeDescription")]
        public string DescricaoTipoDescricao { get; set; }

        [JsonProperty("discountValue")]
        public double? ValorDesconto { get; set; }

        [JsonProperty("protestTypeDescription")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoTipoProtesto { get; set; }

        [JsonProperty("interestTypeDescription")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoTipoJuros { get; set; }

        //Informe a taxa de juros do título.
        [JsonProperty("percentageInterestRate")]
        [Display(Name = "%Tx. Juros")]
        public double? PercentTaxaJuros { get; set; }

        //Informe a quantidade de dias para juros do título.
        [JsonProperty("interestDays")]
        [Display(Name = "Dias Juros")]
        public int? DiasJuros { get; set; }

        //Informe a taxa da multa do título.
        [JsonProperty("percentageRateFine")]
        [Display(Name = "%Tx. Multa")]
        public double? PercentTaxaMulta { get; set; }

        //Informe a quantidade de dias para multa do título.
        [JsonProperty("daysRateFine")]
        [Display(Name = "Dias Multa")]
        public int? DiasMulta { get; set; }

        //Selecione o tipo de protesto do título: Não protestar.                          
        //Protestar dias úteis                    
        //Usar perfil cedente.
        [JsonProperty("typeOfProtest")]
        [Display(Name = "Protesto")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TipoProtesto { get; set; }

        //Informe a quantidade de dias para       
        //protesto do título.
        [JsonProperty("daysOfProtest")]
        [Display(Name = "Dias Protesto")]
        public int? DiasProtesto { get; set; }

        //Informe o sacador avalista do título.
        [JsonProperty("drawerGuarantor")]
        [Display(Name = "Sacador/Aval.")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SacadorAvalista { get; set; }

        //Informe o CNPJ.
        [JsonProperty("cnpjCpf")]
        [Display(Name = "Cnpj")]
        [StringLength(14, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CnpjCpf { get; set; }

        //Campo de mensagem que irá compor o boleto.
        [JsonProperty("receiptWithdrawnMessage")]
        [Display(Name = "Msg.Recibo Sac.")]
        public string MensagemReciboSacado { get; set; }

        //Campo de mensagem que irá compor o boleto.
        [JsonProperty("messageFormCompensation")]
        [Display(Name = "Msg.Ficha Comp.")]
        public string MensagemFichaCompensacao { get; set; }

        //
        [JsonProperty("accountReceivableIds")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public List<String> AccountReceivableIds { get; set; }
        //{
        //    get { return !String.IsNullOrWhiteSpace(this.AccountsIds) ? this.AccountsIds.Split(',').ToList<String>() : new List<string>(); }
        //}

        [JsonIgnore]
        public string AccountsIds { get; set; }
    }
}
