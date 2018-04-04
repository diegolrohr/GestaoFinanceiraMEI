using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Compras.Entities.ViewModel.Base;
using Newtonsoft.Json;
using Fly01.Utils.Helpers;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class NfeCertificateVM : DomainBaseVM
    {
        [JsonProperty("type")]
        [Display(Name = "Tipo de certificado digital ")]
        public string Type { get; set; }

        [JsonProperty("certificate")]
        [Display(Name = "Nome do arquivo do certificado digital ")]
        [StringLength(500, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Certificate { get; set; }

        [JsonProperty("key")]
        [Display(Name = "Nome do arquivo da Private Key ")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Key { get; set; }

        [JsonProperty("password")]
        [Display(Name = "Senha do arquivo digital ")]
        [StringLength(50, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Password { get; set; }

        [Display(Name = "Data de validade")]
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
    }
}
