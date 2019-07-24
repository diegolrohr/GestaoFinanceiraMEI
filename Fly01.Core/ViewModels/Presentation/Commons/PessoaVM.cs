using Fly01.Core.Helpers.Attribute;
using Newtonsoft.Json;
using System;

namespace Fly01.Core.ViewModels.Presentation.Commons
{
    public class PessoaVM : DomainBaseVM
    {
        [JsonProperty("nome")]
        public string Nome { get; set; }

        //Selecione o tipo de CGC CPF/CNPJ
        [JsonProperty("tipoDocumento")]
        public string TipoDocumento { get; set; }

        //Informe o CNPJ se pessoa jurídica ou CPF se pessoa física. Informe apenas números.
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

        [JsonProperty("paisId")]
        public Guid? PaisId { get; set; }

        [JsonProperty("idEstrangeiro")]
        public string IdEstrangeiro { get; set; }

        [JsonProperty("pais")]
        public virtual PaisVM Pais { get; set; }

        [JsonProperty("estado")]
        public virtual EstadoVM Estado { get; set; }

        [JsonProperty("cidade")]
        public virtual CidadeVM Cidade { get; set; }

        [JsonProperty("registroFixo")]
        public bool RegistroFixo { get; set; }

        [JsonProperty("estadoCodigoIbge")]
        public string EstadoCodigoIbge { get; set; }

        [JsonProperty("cidadeCodigoIbge")]
        public string CidadeCodigoIbge { get; set; }

        [JsonProperty("situacaoEspecialNFS")]
        [APIEnum("TipoSituacaoEspecialNFS")]
        public string SituacaoEspecialNFS { get; set; }
    }
}