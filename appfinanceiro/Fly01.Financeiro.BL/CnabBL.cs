using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;

namespace Fly01.Financeiro.BL
{
    public class CnabBL : PlataformaBaseBL<Cnab>
    {
        public CnabBL(AppDataContextBase context) : base(context) { }

        public override void ValidaModel(Cnab entity)
        {
            var banco = Boleto2Net.Banco.Instancia(Boleto2Net.Bancos.BancoDoBrasil);

            banco.Cedente = GetCedenteBoletoNet(entity.ContaBancariaCedente);
            banco.FormataCedente();

            base.ValidaModel(entity);
        }

        private Boleto2Net.Sacado GetContaBancariaSacado(Pessoa sacado)
        {
            return new Boleto2Net.Sacado
            {
                CPFCNPJ = "443.316.101-28",
                Nome = "Sacado Teste PF",
                Observacoes = "Matricula 678/9",
                Endereco = new Boleto2Net.Endereco
                {
                    LogradouroEndereco = "Rua Testando",
                    LogradouroNumero = "456",
                    Bairro = "Bairro",
                    Cidade = "Cidade",
                    UF = "SP",
                    CEP = "56789012"
                }
            };
        }

        private Boleto2Net.Cedente GetCedenteBoletoNet(ContaBancaria contaCedente)
        {
            var dadosEmpresaCedente = RestHelper.ExecuteGetRequest<ManagerEmpresaVM>($"{AppDefaults.UrlGateway}v2/", $"Empresa/{PlataformaUrl}");

            return new Boleto2Net.Cedente
            {
                CPFCNPJ = dadosEmpresaCedente.CNPJ,
                Nome = dadosEmpresaCedente.NomeFantasia,
                Codigo = "0",
                CodigoDV = "0",
                Endereco = new Boleto2Net.Endereco
                {
                    LogradouroEndereco = dadosEmpresaCedente.Endereco,
                    LogradouroNumero = dadosEmpresaCedente.Numero,
                    LogradouroComplemento = "",
                    Bairro = dadosEmpresaCedente.Bairro,
                    Cidade = dadosEmpresaCedente.Cidade?.Nome,
                    UF = dadosEmpresaCedente.Cidade?.Estado?.Sigla,
                    CEP = dadosEmpresaCedente.CEP
                },
                ContaBancaria = new Boleto2Net.ContaBancaria
                {
                    Agencia = contaCedente.Agencia,
                    DigitoAgencia = contaCedente.DigitoAgencia,
                    Conta = contaCedente.Conta,
                    DigitoConta = contaCedente.DigitoConta,
                    CarteiraPadrao = Boleto2Net.CarteiraPadrao.BBCarteira11CobrancaComRegistro.ToString(),
                    VariacaoCarteiraPadrao = "019",
                    TipoCarteiraPadrao = Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = Boleto2Net.TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = Boleto2Net.TipoImpressaoBoleto.Empresa
                }
            };
        }

        //private Boleto2Net.Boleto GetBoleto(Boleto2Net.IBanco banco)
        //{
        //    return new Boleto2Net.Boleto(banco)
        //    {
        //        Sacado = GerarSacado(),
        //        DataEmissao = DateTime.Now.AddDays(-3),
        //        DataProcessamento = DateTime.Now,
        //        DataVencimento = DateTime.Now.AddMonths(i),
        //        ValorTitulo = (decimal)100 * i,
        //        NossoNumero = NossoNumeroInicial == 0 ? "" : (NossoNumeroInicial + _proximoNossoNumero).ToString(),
        //        NumeroDocumento = "BB" + _proximoNossoNumero.ToString("D6") + (char)(64 + i),
        //        EspecieDocumento = TipoEspecieDocumento.DM,
        //        Aceite = aceite,
        //        CodigoInstrucao1 = "11",
        //        CodigoInstrucao2 = "22",
        //        DataDesconto = DateTime.Now.AddMonths(i),
        //        ValorDesconto = (decimal)(100 * i * 0.10),
        //        DataMulta = DateTime.Now.AddMonths(i),
        //        PercentualMulta = (decimal)2.00,
        //        ValorMulta = (decimal)(100 * i * (2.00 / 100)),
        //        DataJuros = DateTime.Now.AddMonths(i),
        //        PercentualJurosDia = (decimal)0.2,
        //        ValorJurosDia = (decimal)(100 * i * (0.2 / 100)),
        //        MensagemArquivoRemessa = "Mensagem para o arquivo remessa",
        //        NumeroControleParticipante = "CHAVEPRIMARIA=" + _proximoNossoNumero
        //    };
        //}
    }
}