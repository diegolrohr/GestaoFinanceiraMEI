using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Entities.ViewModels.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class ParametrosBancariosVM : DomainBaseVM
    {
        //
        [JsonProperty("bankId")]
        [StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string BancoId { get; set; }

        //Código  do agente cobrador segundo o padrão do Banco Central.
        [JsonProperty("bankCode")]
        [Display(Name = "Banco")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Banco { get; set; }

        //Código da agência do agente cobrador.
        [JsonProperty("agencyCode")]
        [Display(Name = "Agencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agencia { get; set; }

        //Número  da  conta  do  usuário, junto ao agente cobrador se existir.
        [JsonProperty("account")]
        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Conta { get; set; }

        //Número  da sub-conta,  código fornecidopelo banco  que pode ou não pertencer aonúmero da conta.
        [JsonProperty("subAccount")]
        [Display(Name = "Sub Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string SubConta { get; set; }

        //Extensão do arquivo, isto é alguns bancos definem o nome da extensão do arquivo. Ex.: .TXT, .SFR.
        [JsonProperty("extension")]
        [Display(Name = "Extensao")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Extensao { get; set; }

        //Indica se o valor da despesa da cobrança está subtraido do valor principal. Caso esteja, deverá ser respondido com "SIM" para que o valor da despesa seja considerado no momento da baixa.
        [JsonProperty("deductsMain")]
        [Display(Name = "Desp Princ.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DespesaPrincipal { get; set; }

        //Tamanho da linha do registro detalhe noarquivo de retorno bancário.
        [JsonProperty("bytesNumber")]
        [Display(Name = "Nro.Bytes")]
        public int? NumeroBytes { get; set; }

        //Formato da data no retorno: 1-ddmmaa, 2=mmddaa, 3=aammdd, 4=ddmmaaaa,5=aaaammdd,6=mmddaaaa.
        [JsonProperty("dateFormat")]
        [Display(Name = "Formato Data")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string FormatoData { get; set; }

        //Categoria Financeira padrão para despesas bancárias
        [JsonProperty("financialCategoryId")]
        [Display(Name = "Categoria De Despesas")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CategoriaId { get; set; }

        [JsonProperty("categoryDescription")]
        public string CategoriaDescricao { get; set; }
    }
}