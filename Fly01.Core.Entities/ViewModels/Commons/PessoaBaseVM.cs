using Fly01.Core.Entities.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.Entities.ViewModels.Commons
{
    public abstract class PessoaBaseVM<TEstado, TCidade> : DomainBaseVM
        where TEstado : EstadoBaseVM
        where TCidade : CidadeBaseVM<TEstado>
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        //Selecione o tipo de CGC CPF/CNPJ
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }

        //Informe o CNPJ se pessoa jurídica ou CPF se pessoa física. Informe apenas       números.
        [JsonProperty("cpfcnpj")]
        public string CPFCNPJ { get; set; }

        //CEP padrão
        [JsonProperty("cep")]
        public string CEP { get; set; }

        //Endereço padrão
        [JsonProperty("endereco")]
        public string Endereco { get; set; }

        //Endereço padrão
        [JsonProperty("numero")]
        public string Numero { get; set; }

        //Endereço padrão
        [JsonProperty("complemento")]
        public string Complemento { get; set; }

        //Informe o bairro.
        [JsonProperty("bairro")]
        public string Bairro { get; set; }

        //Município padrão
        [JsonProperty("cidadeId")]
        public Guid? CidadeId { get; set; }

        //Informe o estado
        [JsonProperty("estadoId")]
        public Guid? EstadoId { get; set; }

        //Informe o Telefone
        [JsonProperty("telefone")]
        public string Telefone { get; set; }

        //Informe o celular.
        [JsonProperty("celular")]
        public string Celular { get; set; }

        //Informe o contato da empresa.
        [JsonProperty("contato")]
        public string Contato { get; set; }

        //Observações gerais.
        [JsonProperty("observacao")]
        public string Observacao { get; set; }


        //Informe o email para de contato.
        [JsonProperty("email")]
        public string Email { get; set; }

        //Informe o nome fantasia da empresa.
        [JsonProperty("nomeComercial")]
        public string NomeComercial { get; set; }

        [JsonProperty("inscricaoEstadual")]
        public string InscricaoEstadual { get; set; }

        [JsonProperty("inscricaoMunicipal")]
        public string InscricaoMunicipal { get; set; }

        [JsonProperty("consumidorFinal")]
        public bool ConsumidorFinal { get; set; }

        //Informe se a pessoa que será cadastrada é uma transportadora.
        [JsonProperty("transportadora")]
        public bool Transportadora { get; set; }

        //Informe se a pessoa que será cadastrada é um cliente.
        [JsonProperty("cliente")]
        public bool Cliente { get; set; }

        //Informe se a pessoa que será cadastrada é um fornecedor.
        [JsonProperty("fornecedor")]
        public bool Fornecedor { get; set; }

        //Informe se a pessoa que será cadastrada é um vendedor.
        [JsonProperty("vendedor")]
        public bool Vendedor { get; set; }

        [JsonProperty("tipoIndicacaoInscricaoEstadual")]
        [APIEnum("TipoIndicacaoInscricaoEstadual")]
        public string TipoIndicacaoInscricaoEstadual { get; set; }

        #region NavigationProperties

        [JsonProperty("estado")]
        public virtual TEstado Estado { get; set; }

        [JsonProperty("cidade")]
        public virtual TCidade Cidade { get; set; }

        #endregion
    }
}
