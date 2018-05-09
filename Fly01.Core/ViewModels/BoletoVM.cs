using System;

namespace Fly01.Core.ViewModels
{
    public sealed class BoletoVM
    {
        public decimal ValorPrevisto { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal ValorJuros { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NossoNumero { get; set; }
        public string EspecieMoeda { get; set; }
        public string NumeroDocumento { get; set; }
        public string InstrucoesCaixa { get; set; }

        public CedenteVM Cedente { get; set; }
        public SacadoVM Sacado { get; set; }
    }

    public sealed class CedenteVM
    {
        public string CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public string EnderecoCidade { get; set; }
        public string EnderecoUF { get; set; }
        public string EnderecoCEP { get; set; }
        public string Observacoes { get; set; }
        public string CodigoCedente { get; set; }

        public ContaBancariaCedenteVM ContaBancariaCedente { get; set; }
    }

    public sealed class ContaBancariaCedenteVM
    {
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public int CodigoBanco { get; set; }
    }

    public sealed class SacadoVM
    {
        public string CNPJ { get; set; }
        public string Nome { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string EnderecoBairro { get; set; }
        public string EnderecoCidade { get; set; }
        public string EnderecoUF { get; set; }
        public string EnderecoCEP { get; set; }
        public string Observacoes { get; set; }

    }
}