using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Compras.Entities.ViewModel.Base;
using Newtonsoft.Json;

namespace Fly01.Compras.Entities.ViewModel
{
    [Serializable]
    public class BranchVM : DomainBaseVM
    {

        [JsonProperty("name")]
        [Display(Name = "Razão social")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Name { get; set; }

        [JsonProperty("tradingName")]
        [Display(Name = "Nome Fantasia")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TradingName { get; set; }

        //Endereço padrão
        [JsonProperty("address")]
        [Display(Name = "Endereço")]
        [StringLength(50, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Address { get; set; }

        //Município padrão
        [JsonProperty("city")]
        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(35, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string City { get; set; }

        //Informe o estado
        [JsonProperty("state")]
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string State { get; set; }

        //Município padrão - Descrição
        [JsonProperty("cityDescricao")]
        [Display(Name = "Cidade Descrição")]
        public string CityDescricao
        {
            get
            {
                return City;
            }
        }


        //CEP padrão
        [JsonProperty("zipcode")]
        [Display(Name = "Cep")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string ZipCode { get; set; }

        //Informe o bairro.
        [JsonProperty("neighborhood")]
        [Display(Name = "Bairro")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Neighborhood { get; set; }

        //Informe o Telefone
        [JsonProperty("phone")]
        [Display(Name = "Telefone Comercial")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Phone { get; set; }

        //Informe o fax.
        [JsonProperty("fax")]
        [Display(Name = "Fax")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Fax { get; set; }

        //Informe o CNPJ se pessoa jurídica ou CPF se pessoa física. Informe apenas       números.
        [JsonProperty("cnpj")]
        [Display(Name = "CNPJ da Empresa")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string CNPJ { get; set; }

        //Informe a Inscrição Estadual
        [JsonProperty("stateInscription")]
        [Display(Name = "Inscrição Estadual")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string StateInscription { get; set; }

        //Número da inscrição municipal ou código do registro de autonomo se pessoa física.
        [JsonProperty("municipalInscription")]
        [Display(Name = "Inscrição Municipal")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string MunicipalInscription { get; set; }

        [JsonProperty("stateInscriptionType")]
        [Display(Name = "Tipo Inscr. Estadual")]
        //[Range(0, 9, ErrorMessage = "O campo {0} deve possuir valores entre {1} e {2}")]
        //[Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public int? StateInscriptionType { get; set; }

        ////Informe se é optante de simples nacional
        //[JsonProperty("simplesNacional")]
        //[Display(Name = "Simples Nacional")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string SimplesNacional { get; set; }

        //Informe se a pessoa que será cadastrada é um cliente.
        //Informe se é optante de simples nacional
        [JsonProperty ("simplesNacionalBool")]
        [Display(Name = "Simples Nacional?")]
        public bool SimplesNacionalBool { get; set; }

        [JsonProperty("simplesNacional")]
        public string SimplesNacionalRest
        {
            get { return SimplesNacionalBool ? "S" : "N"; }
            set { SimplesNacionalBool = (value == "S"); }
        }

        //Informe o email para de contato.
        [JsonProperty("email")]
        [Display(Name = "E-Mail")]
        [StringLength(70, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Email { get; set; }

        ////Informe o código do Município.
        //[JsonProperty("ibgeCityCode")]
        //[Display(Name = "Cod Municipio")]
        //[StringLength(7, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string IbgeCityCode { get; set; }

        [JsonProperty("nire")]
        [Display(Name = "NIRE")]
        [StringLength(25, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Nire { get; set; }

        [JsonIgnore]
        [Display(Name = "Data do NIRE")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NireDate { get; set; }

        [JsonProperty("nireDate")]
        public string NireDateRest
        {
            get
            {
                return NireDate.HasValue ? NireDate.Value.ToString("yyyyMMdd") : string.Empty;
            }
            set
            {
                NireDate = string.IsNullOrWhiteSpace(value) ? (DateTime?)null : value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
            }
        }

        [JsonProperty("cnae")]
        [Display(Name = "CNAE Principal")]
        public string CNAE { get; set; }

        [JsonProperty("active")]
        [Display(Name = "Ativo")]
        public string Active { get; set; }
    }
}
