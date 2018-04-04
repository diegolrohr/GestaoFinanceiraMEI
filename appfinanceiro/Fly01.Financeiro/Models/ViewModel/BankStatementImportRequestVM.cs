using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Fly01.Core;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class BankStatementImportRequestVM : DomainBaseVM
    {
        [JsonProperty("bankId")]
        public string BankId { get; set; }

        [JsonProperty("items")]
        public List<BankStatementImportRequestItemVM> Items { get; set; }

        public BankStatementImportRequestVM()
        {
            Items = new List<BankStatementImportRequestItemVM>();
        }
    }

    public class BankStatementImportRequestItemVM
    {
        [JsonProperty("item")]
        public string Item { get; set; }

        [Display(Name = "Data")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [JsonProperty("date")]
        public string DateRest
        {
            get
            {
                return Date.HasValue ? Date.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                Date = value.ToDateTime();
            }
        }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonIgnore]
        public string ValueFormatted { get { return Value.ToString("C", AppDefaults.CultureInfoDefault); } }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("details")]
        public List<BankStatementImportRequestItemDetail> Details { get; set; }

        public BankStatementImportRequestItemVM()
        {
            Details = new List<BankStatementImportRequestItemDetail>();
        }
    }

    public class BankStatementImportRequestItemDetail
    {
        [JsonProperty("detail")]
        public string Detail { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty("accountInstallment")]
        public string AccountInstallment { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("personId")]
        public string PersonId { get; set; }

        [JsonProperty("personName")]
        public string PersonName { get; set; }

        [JsonProperty("accountSeries")]
        public string AccountSeries { get; set; }

        [JsonProperty("accountOrigBranch")]
        public string AccountOrigBranch { get; set; }

        [JsonProperty("description")]
        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonIgnore]
        public string ValueFormatted { get { return Value.ToString("C", AppDefaults.CultureInfoDefault); } }

        [Display(Name = "Vencimento")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DueDate { get; set; }

        [JsonProperty("dueDate")]
        public string DueDateRest
        {
            get
            {
                return DueDate.HasValue ? DueDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                DueDate = value.ToDateTime();
            }
        }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("categoryDescription")]
        public string CategoryDescription { get; set; }

        [JsonProperty("add")]
        public bool Add { get; set; }

        [JsonProperty("update")]
        public bool Update { get; set; }

        [JsonProperty("delete")]
        public bool Delete { get; set; }
    }
}