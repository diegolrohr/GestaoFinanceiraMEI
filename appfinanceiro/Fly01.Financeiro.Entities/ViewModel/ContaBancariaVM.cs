using System;
using System.ComponentModel.DataAnnotations;
using Fly01.Core.ViewModels.Presentation.Commons;
using Newtonsoft.Json;

namespace Fly01.Financeiro.Entities.ViewModel
{
    [Serializable]
    public class ContaBancariaVM : DomainBaseVM
    {
        [JsonProperty("bancoId")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Banco")]
        public Guid? BancoId { get; set; }

        [JsonProperty("banco")]
        [Display(Name = "Banco")]
        public virtual BancoVM Banco { get; set; }

        //Número da agência
        [JsonProperty("agencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Agência")]
        [StringLength(5, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Agencia { get; set; }

        //Informe o dígito da agência.
        [JsonProperty("digitoAgencia")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Dígito da Agência")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DigitoAgencia { get; set; }

        //Número da conta
        [JsonProperty("conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Conta")]
        [StringLength(10, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string Conta { get; set; }

        //Informe o dígito da conta.
        [JsonProperty("digitoConta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [Display(Name = "Dígito da Conta")]
        [StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string DigitoConta { get; set; }

        //Nome do banco
        [JsonProperty("nomeConta")]
        [Display(Name = "Nome da Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        public string NomeConta { get; set; }
        
        //CRUD NOVA API V2

        ////Endereço do banco
        //[JsonProperty("endereco")]
        //[Display(Name = "Endereco")]
        //[StringLength(40, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Endereco { get; set; }

        ////Bairro onde se localiza o banco
        //[JsonProperty("bairro")]
        //[Display(Name = "Bairro")]
        //[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Bairro { get; set; }

        ////Municipio onde se localiza o banco
        //[JsonProperty("cidade")]
        //[Display(Name = "Municipio")]
        //[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Cidade { get; set; }

        ////CEP do banco
        //[JsonProperty("cep")]
        //[Display(Name = "Cep")]
        //[StringLength(8, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Cep { get; set; }

        ////Estado onde se localiza o banco
        //[JsonProperty("estado")]
        //[Display(Name = "Estado")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Estado { get; set; }

        ////Telefone para contato
        //[JsonProperty("fone")]
        //[Display(Name = "Telefone")]
        //[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Telefone { get; set; }

        ////Contato no banco
        //[JsonProperty("contato")]
        //[Display(Name = "Contato")]
        //[StringLength(45, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Contato { get; set; }

        ////Considera no fluxo de caixa
        //[JsonProperty("fluxoCaixa")]
        //[Display(Name = "Cons. Fluxo")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string FluxoCaixa { get; set; }

        ////Informe se o dados deste banco aparecerá na tela com os indicadores de gestão.
        //[JsonProperty("indiceGestao")]
        //[Display(Name = "Indic.Gestao ?")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string IndicadorGestao { get; set; }

        ////Filial de origem
        //[JsonProperty("filialOrigem")]
        //[Display(Name = "Fil. Origem")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string FilialOrigem { get; set; }

        ////Informe a conta contábil de débito do banco
        //[JsonProperty("contaDebito")]
        //[Display(Name = "Cta Debito")]
        //[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string ContaDebito { get; set; }

        ////Informe a conta contábil de crédito do banco
        //[JsonProperty("contaCredito")]
        //[Display(Name = "Cta Credito")]
        //[StringLength(20, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string ContaCredito { get; set; }

        ////Percentual a ser aplicado no valor de um título para determinar o custo para operação de desconto.
        //[JsonProperty("taxaDesconto")]
        //[Display(Name = "Taxa Descon.")]
        //public double? TaxaDesconto { get; set; }

        ////Número de dias que o agente cobrador demora para creditar na conta da empresa 
        ////os valores referentes a operação de desconto nos títulos
        //[JsonProperty("diasRetencaoDesconto")]
        //[Display(Name = "Ret.P/Descon")]
        //public int? DiasRetencaoDesconto { get; set; }

        ////Define se o banco será padrão do sistema.
        //[JsonProperty("bancoPadraoSistema")]
        //[Display(Name = "Define Se O Banco Será Padrão Do Sistema")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string BancoPadraoSistema { get; set; }

        ////Arquivo de Layout do boleto.
        //[JsonProperty("arquivoLayoutBoleto")]
        //[Display(Name = "Arquivo De Layout Do Boleto")]
        //[StringLength(100, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string ArquivoLayoutBoleto { get; set; }

        ////Número do próximo título a ser gerado com este banco
        //[JsonProperty("nossoNumero")]
        //[Display(Name = "Nosso Numero")]
        //[StringLength(15, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string NossoNumero { get; set; }

        ////Numero Sequencial adotado na nomenclatura do arquivo CNAB.
        //[JsonProperty("numeroSequencialCNAB")]
        //[Display(Name = "Seq. Arq. Cnab")]
        //[StringLength(6, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string NumeroSequencialCNAB { get; set; }

        ////Número do cedente para boletos do banco Santander
        //[JsonProperty("codigoCedente")]
        //[Display(Name = "Cod.Cedente")]
        //[StringLength(7, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string CodigoCedente { get; set; }

        ////Informe a situação do CNAB
        //[JsonProperty("situacaoCNAB")]
        //[Display(Name = "Situacao")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string SituacaoCNAB { get; set; }

        ////Descrição da situaçao de cobrança.
        //[JsonProperty("descricaoSituacaoCNAB")]
        //[Display(Name = "Descr.Sit.")]
        //[Required(ErrorMessage = "O campo {0} é obrigatório.")]
        //[StringLength(25, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoSituacaoCNAB { get; set; }

        ////Informe a primeira instrução de cobrança
        //[JsonProperty("instrucaoCNAB1")]
        //[Display(Name = "1A.Instr.Cnab")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string InstrucaoCNAB1 { get; set; }

        ////
        //[JsonProperty("descricaoInstrucaoCNAB1")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoInstrucaoCNAB1 { get; set; }

        ////Informe a segunda intsrução de cobrança
        //[JsonProperty("instrucaoCNAB2")]
        //[Display(Name = "2A.Instr.Cnab")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string InstrucaoCNAB2 { get; set; }

        ////
        //[JsonProperty("descricaoInstrucaoCNAB2")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoInstrucaoCNAB2 { get; set; }

        ////Informe o local da impressão do título.
        //[JsonProperty("localImpressao")]
        //[Display(Name = "Loc.Impressao")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string LocalImpressao { get; set; }

        ////Selecione o tipo da carteria:          
        ////Simples sem registro.                   
        ////Simples com registro.                   
        ////Penhor com registro.
        //[JsonProperty("carteira")]
        //[Display(Name = "Carteira")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Carteira { get; set; }

        ////
        //[JsonProperty("descricaoCarteira")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoCarteira { get; set; }

        ////Informe a especie do título.
        //[JsonProperty("especie")]
        //[Display(Name = "Especie")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Especie { get; set; }

        ////Informe o valor do abatimento do título.
        //[JsonProperty("valorAbatimento")]
        //[Display(Name = "Vlr Abatimento")]
        //public double? ValorAbatimento { get; set; }

        ////Informe o valor de IOF do título.
        //[JsonProperty("valorIOF")]
        //[Display(Name = "Valor Iof")]
        //public double? ValorIOF { get; set; }

        ////Selecione o tipo do desconto do título:    
        ////Não consta desconto.                 
        ////Valor fixo até a data informada.        
        ////Percentual até a data informada.        
        ////Desconto por dia corrido.               
        ////Desconto por dia útil.
        //[JsonProperty("tipoDesconto")]
        //[Display(Name = "Tp.Desc.")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string TipoDesconto { get; set; }

        ////Informe o valor de desconto do título.
        //[JsonProperty("valorDesconto")]
        //[Display(Name = "Vlr.Desc.")]
        //public double? ValorDesconto { get; set; }

        ////Informe a data do desconto do título.
        //[Display(Name = "Dta.Desconto")]
        //[JsonIgnore]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? DataDesconto { get; set; }

        //[JsonProperty("dataDesconto")]
        //public string DataDescontoRest
        //{
        //    get
        //    {
        //        return DataDesconto.HasValue ? DataDesconto.Value.ToString("yyyyMMdd") : null;
        //    }
        //    set
        //    {
        //        DataDesconto = value.ToDateTime(Extensions.DateFormat.YYYYMMDD);
        //    }
        //}

        ////Informe o tipo de juros do título
        //[JsonProperty("tipoJuros")]
        //[Display(Name = "Tipo Juros")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string TipoJuros { get; set; }

        ////
        //[JsonProperty("descricaoTipoJuros")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoTipoJuros { get; set; }

        ////Informe a taxa de juros do título.
        //[JsonProperty("percentTaxaJuros")]
        //[Display(Name = "%Tx. Juros")]
        //public double? PercentTaxaJuros { get; set; }

        ////Informe a quantidade de dias para juros do título.
        //[JsonProperty("diasJuros")]
        //[Display(Name = "Dias Juros")]
        //public int? DiasJuros { get; set; }

        ////Informe a taxa da multa do título.
        //[JsonProperty("percentTaxaMulta")]
        //[Display(Name = "%Tx. Multa")]
        //public double? PercentTaxaMulta { get; set; }

        ////Informe a quantidade de dias para multa do título.
        //[JsonProperty("diasMulta")]
        //[Display(Name = "Dias Multa")]
        //public int? DiasMulta { get; set; }

        ////Selecione a opção de baixa do título:   
        ////Baixar.                                 
        ////Não Baixar.                             
        ////Usar perfil cedente.
        //[JsonProperty("baixa")]
        //[Display(Name = "Baixa")]
        //[StringLength(2, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string Baixa { get; set; }

        ////
        //[JsonProperty("descricaoBaixa")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoBaixa { get; set; }

        ////Informe a quantidade de dias para baixa do título.
        //[JsonProperty("diasBaixa")]
        //[Display(Name = "Dias Baixa")]
        //public int? DiasBaixa { get; set; }

        ////Selecione o tipo de protesto do título: Não protestar.                          
        ////Protestar dias úteis                    
        ////Usar perfil cedente.
        //[JsonProperty("tipoProtesto")]
        //[Display(Name = "Protesto")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string TipoProtesto { get; set; }

        ////
        //[JsonProperty("descricaoTipoProtesto")]
        //[StringLength(0, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string DescricaoTipoProtesto { get; set; }

        ////Informe a quantidade de dias para protesto do título.
        //[JsonProperty("diasProtesto")]
        //[Display(Name = "Dias Protesto")]
        //public int? DiasProtesto { get; set; }

        ////
        //[JsonProperty("integrarPDV")]
        //[Display(Name = "Integrar Com Pdv")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string IntegrarPDV { get; set; }

        ////
        //[JsonProperty("ativoIntegracaoPDV")]
        //[Display(Name = "Ativo Integracao Pdv")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string AtivoIntegracaoPDV { get; set; }

        ////
        //[JsonProperty("caixaGeral")]
        //[Display(Name = "Caixa Geral")]
        //[StringLength(1, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string CaixaGeral { get; set; }

        ////
        //[JsonProperty("codigoPDV")]
        //[Display(Name = "Codigo Do Pdv")]
        //[StringLength(3, ErrorMessage = "O campo {0} deve possuir entre {2} e {1} caracteres.")]
        //public string CodigoPDV { get; set; }

        //[JsonIgnore]
        //public ParametrosBancariosVM ParametrosBanco { get; set; }

        ////[JsonProperty("parametersBank", NullValueHandling = NullValueHandling.Ignore)]
        //[JsonProperty("parametrosBancoRequest")]
        //public List<ParametrosBancariosVM> ParametrosBancoRequest { get; set; }

        //[JsonProperty("agreementBankBusiness")]
        //public List<AgreementbankBusinessVM> AgreementbankBusiness { get; set; }
    }
}