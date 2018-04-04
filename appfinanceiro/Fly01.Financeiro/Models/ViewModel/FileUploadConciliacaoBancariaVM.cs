using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.Helpers;

using Newtonsoft.Json;

namespace Fly01.Financeiro.Models.ViewModel
{
    [Serializable]
    public class FileUploadConciliacaoBancariaVM
    {
        [JsonProperty("fileContent")]
        public string FileContent { get; set; }

        [JsonProperty("fileMD5")]
        public string FileMD5 { get; set; }

        [JsonProperty("BankId")]
        public string BankId { get; set; }
        
        [JsonIgnore]
        public string BankName { get; set; }

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
                ImportDate = value.ToDateTime();
            }
        }
    }
}