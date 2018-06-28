using System;

namespace Fly01.Core.ViewModels
{
    public sealed class BoletoVM
    {
        public decimal ValorPrevisto { get; set; }
        public decimal ValorMulta { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal ValorJuros { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataDesconto { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public DateTime? DataCredito { get; set; }

        public string VariacaoCarteira { get; set; }
        public string Carteira { get; set; } = string.Empty;

        public string RegistroArquivoRetorno { get; set; } 
        public int NossoNumero { get; set; }
        public string EspecieMoeda { get; set; }
        public string NumeroDocumento { get; set; }
        public string InstrucoesCaixa { get; set; }
        public string NossoNumeroDV { get; set; }
        public string NossoNumeroFormatado { get; set; }
        public string CodigoOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }
        public string CodigoOcorrenciaAuxiliar { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorTarifas { get; set; }
        public decimal ValorOutrasDespesas { get; set; }
        public decimal ValorOutrosCreditos { get; set; }
        public decimal ValorIOF { get; set; }
        public decimal ValorAbatimento { get; set; }

        // Verificar EspecieDocumento
        //public TipoEspecieDocumento EspecieDocumento { get; set; } = TipoEspecieDocumento.NaoDefinido;
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
        public string CodigoDV { get; set; }

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