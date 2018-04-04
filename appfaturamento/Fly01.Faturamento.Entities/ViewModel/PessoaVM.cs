using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.VM;
using Newtonsoft.Json;
using Fly01.Core.Api;

namespace Fly01.Faturamento.Entities.ViewModel
{
    [Serializable]
    public class PessoaVM : DomainBaseVM
    {
        [JsonProperty("nome")]
        [Display(Name = "Razão social")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Nome { get; set; }

        //Selecione o tipo de CGC CPF/CNPJ
        [JsonProperty("tipoDocumento")]
        [Display(Name = "Tipo de documento")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string TipoDocumento { get; set; }

        //Informe o CNPJ se pessoa jurídica ou CPF se pessoa física. Informe apenas       números.
        [JsonProperty("cpfcnpj")]
        [Display(Name = "CPF/CNPJ")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CPFCNPJ { get; set; }

        //CEP padrão
        [JsonProperty("cep")]
        [StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string CEP { get; set; }

        //Endereço padrão
        [JsonProperty("endereco")]
        [StringLength(50, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Endereco { get; set; }

        //Endereço padrão
        [JsonProperty("numero")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Numero { get; set; }

        //Endereço padrão
        [JsonProperty("complemento")]
        [StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Complemento { get; set; }

        //Informe o bairro.
        [JsonProperty("bairro")]
        [StringLength(30, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Bairro { get; set; }

        //Município padrão
        [JsonProperty("cidadeId")]
        public Guid? CidadeId { get; set; }

        //Informe o estado
        [JsonProperty("estadoId")]
        public Guid? EstadoId { get; set; }

        //Informe o Telefone
        [JsonProperty("telefone")]
        [Display(Name = "Telefone comercial")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Telefone { get; set; }

        //Informe o celular.
        [JsonProperty("celular")]
        [Display(Name = "Telefone celular")]
        [StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Celular { get; set; }

        //Informe o contato da empresa.
        [JsonProperty("contato")]
        [Display(Name = "Pessoa de contato")]
        [StringLength(45, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Contato { get; set; }

        //Observações gerais.
        [JsonProperty("observacao")]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }


        //Informe o email para de contato.
        [JsonProperty("email")]
        [Display(Name = "E-mail")]
        [StringLength(70, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Email { get; set; }

        //Informe o nome fantasia da empresa.
        [JsonProperty("nomeComercial")]
        [Display(Name = "Nome comercial")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeComercial { get; set; }

        [JsonProperty("inscricaoEstadual")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InscricaoEstadual { get; set; }

        [JsonProperty("inscricaoMunicipal")]
        [StringLength(18, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string InscricaoMunicipal { get; set; }

        [JsonProperty("consumidorFinal")]
        [Display(Name = "Consumidor Final")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool ConsumidorFinal { get; set; }

        //Informe se a pessoa que será cadastrada é uma transportadora.
        [JsonProperty("transportadora")]
        [Display(Name = "Transportadora")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Transportadora { get; set; }

        //Informe se a pessoa que será cadastrada é um cliente.
        [JsonProperty("cliente")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Cliente { get; set; }

        //Informe se a pessoa que será cadastrada é um fornecedor.
        [JsonProperty("fornecedor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Fornecedor { get; set; }

        //Informe se a pessoa que será cadastrada é um vendedor.
        [JsonProperty("vendedor")]
        [Display(Name = "Vendedor")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public bool Vendedor { get; set; }

        [JsonProperty("tipoIndicacaoInscricaoEstadual")]
        [APIEnum("TipoIndicacaoInscricaoEstadual")]
        public string TipoIndicacaoInscricaoEstadual { get; set; }

        #region NavigationProperties

        [JsonProperty("estado")]
        public virtual EstadoVM Estado { get; set; }

        [JsonProperty("cidade")]
        public virtual CidadeVM Cidade { get; set; }

        #endregion
    }
}