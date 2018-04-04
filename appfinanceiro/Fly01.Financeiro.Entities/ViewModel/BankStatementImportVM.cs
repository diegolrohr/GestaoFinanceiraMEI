using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;
using Fly01.Core.VM;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    public class BankStatementImportVM : DomainBaseVM
    {
        [JsonProperty("BankId")]
        public string BankId { get; set; }

        [JsonProperty("bankCode")]
        public string BankCode { get; set; }

        [JsonProperty("bankAgency")]
        public string BankAgency { get; set; }

        [JsonProperty("bankAccount")]
        public string BankAccount { get; set; }

        [JsonProperty("bankName")]
        public string BankName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [Display(Name = "Data Importação")]
        [JsonIgnore]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ImportDate { get; set; }

        [JsonProperty("importDate")]
        public string ImportDateRest
        {
            get
            {
                return ImportDate.HasValue ? ImportDate.Value.ToString("yyyyMMdd") : null;
            }
            set
            {
                ImportDate = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }
    }
}
