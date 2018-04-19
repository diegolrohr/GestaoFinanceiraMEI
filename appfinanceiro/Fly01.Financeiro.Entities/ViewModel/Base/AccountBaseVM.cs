using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;
using Fly01.Core.Helpers;

namespace Fly01.Financeiro.Entities.ViewModel.Base
{
    [Serializable]
    public abstract class AccountBaseVM : DomainBaseVM
    {
        public abstract string GetChequeTypeOperation();
        public abstract string GetAdiantamentoTypeOperation();

        [JsonProperty("installment")]
        [Display(Name = "Parcela")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Parcela { get; set; }

        [JsonProperty("type")]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Tipo { get; set; }

        [JsonProperty("personId")]
        [Display(Name = "Cod.Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CodPessoa { get; set; }

        [JsonProperty("personName")]
        [Display(Name = "Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Pessoa { get; set; }

        [JsonProperty("series")]
        [Display(Name = "Serie")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Serie { get; set; }

        [JsonProperty("categoryId")]
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CategoriaId { get; set; }

        [JsonProperty("categoryDescription")]
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Categoria { get; set; }

        [JsonProperty("installmentDescription")]
        [Display(Name = "Parcela")]
        public string DescricaoParcela { get; set; }

        [Display(Name = "Emissão")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataEmissao { get; set; }

        [JsonProperty("issueDate")]
        public string DataEmissaoREST
        {
            get
            {
                return DataEmissao.HasValue ? DataEmissao.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DataEmissao = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [Display(Name = "Vencimento")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataVencimento { get; set; }

        [JsonProperty("dueDate")]
        public string DataVencimentoREST
        {
            get
            {
                return DataVencimento.HasValue ? DataVencimento.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DataVencimento = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [Display(Name = "Última baixa")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataUltimaBaixa { get; set; }

        [JsonProperty("lastPaymentDate")]
        public string DataUltimaBaixaREST
        {
            get
            {
                return DataUltimaBaixa.HasValue ? DataUltimaBaixa.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DataUltimaBaixa = value.ToDateTime();
            }
        }

        [Display(Name = "Vencto.Orig.")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataVencimentoOriginal { get; set; }

        [JsonProperty("originalDueDate")]
        public string DataVencimentoOriginalREST
        {
            get
            {
                return DataVencimentoOriginal.HasValue ? DataVencimentoOriginal.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DataVencimentoOriginal = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [Display(Name = "Vencto.Real")]
        [JsonIgnore]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DataVencimentoReal { get; set; }

        [JsonProperty("definiteDueDate")]
        public string DataVencimentoRealREST
        {
            get
            {
                return DataVencimentoReal.HasValue ? DataVencimentoReal.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DataVencimentoReal = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [JsonProperty("billValue")]
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public double Valor { get; set; }

        [JsonProperty("irrfValue")]
        [Display(Name = "Vlr.Irrf")]
        public double? ValorIRRF { get; set; }

        [JsonProperty("discount")]
        [Display(Name = "Vlr.Desc.")]
        public double? Desconto { get; set; }

        [JsonProperty("interest")]
        [Display(Name = "Juros")]
        public double? Juros { get; set; }

        [JsonProperty("balance")]
        [Display(Name = "Saldo")]
        public double? Saldo { get; set; }

        [JsonProperty("bankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BancoId { get; set; }

        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Banco { get; set; }

        [JsonProperty("agencyNumber")]
        [Display(Name = "Nro Agencia")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agencia { get; set; }

        [JsonProperty("accountNum")]
        [Display(Name = "Nro Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Conta { get; set; }

        [JsonProperty("agreement")]
        [Display(Name = "Convenio")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Convenio { get; set; }

        [JsonProperty("description")]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Descricao { get; set; }

        [JsonProperty("currency")]
        [Display(Name = "Moeda")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Moeda { get; set; }

        [JsonProperty("origBranch")]
        [Display(Name = "Fil. Origem")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FilialOrigem { get; set; }

        [JsonProperty("cashFlow")]
        [Display(Name = "Fluxo De Caixa")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FluxoCaixa { get; set; }

        [JsonProperty("note")]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [JsonProperty("cnabGenerates")]
        [Display(Name = "Gera Cnab")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string GeraCNAB { get; set; }

        [JsonProperty("numberBordero")]
        [Display(Name = "Num.Bordero")]
        [StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NumeroBordero { get; set; }

        [JsonProperty("idCnab")]
        [Display(Name = "Id Cnab")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string IdCnab { get; set; }

        [JsonProperty("statusWriteOff")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string StatusWriteOff { get; set; }

        [JsonProperty("titleStatus")]
        [Display(Name = "Status Do Titulo")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string StatusTitulo { get; set; }

        [JsonProperty("apportionment")]
        [Display(Name = "Rateio Visualiz.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Rateio { get; set; }

        [JsonProperty("documentCheckNumber")]
        [Display(Name = "Doc.Cheque")]
        [StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DocumentoCheque { get; set; }

        [JsonProperty("advanceMoneyBankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AdvanceMoneyBankId { get; set; }

        [JsonProperty("advanceMoneyCheck")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AdvanceMoneyCheck { get; set; }

        [JsonProperty("advanceMoneyNote")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string AdvanceMoneyNote { get; set; }

        [JsonProperty("subtitleCode")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SubtitleCode { get; set; }

        [JsonProperty("yourNumber")]
        [Display(Name = "Seu Numero")]
        [StringLength(21, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SeuNumero { get; set; }

        [JsonProperty("paymentFormId")]
        [Display(Name = "Forma Pagto")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FormaPagto { get; set; }

        [JsonProperty("paymentFormDescription")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DescricaoFormaPagto { get; set; }

        [JsonProperty("conditionId")]
        [Display(Name = "Cond. De Pagamento")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CondicaoPagtoId { get; set; }

        [JsonProperty("conditionDescription")]
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CondicaoPagtoDescricao { get; set; }

        [JsonProperty("parentId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ParentId { get; set; }

        [JsonProperty("installmentCount")]
        [Display(Name = "Total De Parcelas")]
        public int? InstallmentCount { get; set; }

        [JsonProperty("source")]
        [Display(Name = "Origem")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Origem { get; set; }

        [JsonProperty("code")]
        [Display(Name = "Código")]
        [StringLength(9, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Codigo { get; set; }

        [JsonProperty("rowId")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [JsonProperty("scheduledFrequency")]
        [Display(Name = "Periodicidade")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Periodicidade { get; set; }

        [JsonProperty("scheduledCount")]
        [Display(Name = "Número de repetições")]
        public int? ScheduledCount { get; set; }

        [JsonProperty("scheduledInclusionType")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ScheduledInclusionType { get; set; }

        [JsonIgnore]
        [Display(Name = "Habilitar Recorrência")]
        public bool HabilitarRecorrencia { get; set; }
    }
}
