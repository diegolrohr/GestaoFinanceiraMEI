using Fly01.Core;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Rest;
using Fly01.Core.ViewModels;
using System;

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

            var sacado = GetSacado(entity.Sacado);
            var boleto = new Boleto2Net.BoletoBancario();
            var dadosBoleto = boleto.Boleto.CodigoBarra;

            dadosBoleto.CodigoBanco = "";
            dadosBoleto.Moeda = 9;
            dadosBoleto.FatorVencimento = 0;
            dadosBoleto.ValorDocumento = (entity.ValorBoleto - ((entity.DataDesconto <= DateTime.Now) ? entity.ValorDesconto : 0)).ToString("C", AppDefaults.CultureInfoDefault);
            dadosBoleto.CampoLivre = "";

            //var boletos = GetBoletos(banco, sacado, entity);

            base.ValidaModel(entity);
        }

        private Boleto2Net.Sacado GetSacado(Pessoa sacado)
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

        //private Boleto2Net.Boleto GetBoletosGeradosParaRemessa(Boleto2Net.IBanco banco, Boleto2Net.Sacado sacado, Cnab dadosBoleto)
        //{
        //    //For para ler todos os boletos pendentes

        //    return new Boleto2Net.Boleto(banco)
        //    {
        //        Sacado = sacado,
        //        DataEmissao = dadosBoleto.DataEmissao,
        //        DataProcessamento = DateTime.Now,
        //        DataVencimento = dadosBoleto.DataVencimento,
        //        ValorTitulo = (decimal)dadosBoleto.ValorBoleto,
        //        NossoNumero = dadosBoleto.NossoNumero,
        //        NumeroDocumento = "BB" + _proximoNossoNumero.ToString("D6") + (char)(64 + i),
        //        EspecieDocumento = Boleto2Net.TipoEspecieDocumento.DM,
        //        Aceite = "?",
        //        CodigoInstrucao1 = "11",
        //        CodigoInstrucao2 = "22",
        //        DataDesconto = dadosBoleto.DataDesconto,
        //        ValorDesconto = (decimal)dadosBoleto.ValorDesconto,
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