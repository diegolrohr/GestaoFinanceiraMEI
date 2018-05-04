using System;
using System.Text;
using Fly01.Core.BL;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Text.RegularExpressions;

namespace Fly01.Financeiro.BL
{
    public class CnabBL : PlataformaBaseBL<Cnab>
    {
        protected ContaReceberBL ContaReceber;
        protected ContaBancariaBL ContaBancaria;
        private Boleto2Net.Boletos boletos;
        const double jurosDia = 0.33;
        const double percentMulta = 2.00;

        public CnabBL(AppDataContextBase context, ContaReceberBL contaReceberBL, ContaBancariaBL contaBancariaBL) : base(context)
        {
            boletos = new Boleto2Net.Boletos();
            ContaReceber = contaReceberBL;
            ContaBancaria = contaBancariaBL;
        }

        public Boleto2Net.Boletos GeraBoletos(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        {
            var contaReceber = ContaReceber.Find(contaReceberId);
            if (contaReceber == null) throw new BusinessException("A conta a receber informada não foi encontrada.");

            var contaBancaria = ContaBancaria.Find(contaBancariaId);
            if (contaBancaria == null) throw new BusinessException("A conta bancária informada não foi encontrada.");

            if (!contaBancaria.Banco.EmiteBoleto) throw new BusinessException("Não é possível emitir boletos para esta instituição bancária.");

            var banco = Boleto2Net.Banco.Instancia(ushort.Parse(contaBancaria.Banco.Codigo));

            banco.Cedente = GetCedenteBoletoNet(contaBancaria);
            banco.FormataCedente();

            MontaBoleto(banco, contaReceber, dataDesconto, valorDesconto);

            return boletos;
        }

        private Boleto2Net.Sacado GetSacado(Pessoa sacado)
        {
            return new Boleto2Net.Sacado
            {
                CPFCNPJ = sacado.CPFCNPJ,
                Nome = sacado.Nome,
                Observacoes = "",
                Endereco = new Boleto2Net.Endereco
                {
                    LogradouroEndereco = sacado.Endereco,
                    LogradouroNumero = sacado.Numero,
                    Bairro = sacado.Bairro,
                    Cidade = sacado.Cidade.Nome,
                    UF = sacado.Cidade.Estado.Sigla,
                    CEP = sacado.CEP
                }
            };
        }

        private Boleto2Net.Cedente GetCedenteBoletoNet(ContaBancaria contaCedente)
        {
            var dadosEmpresaCedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);

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
                    CarteiraPadrao = /*Boleto2Net.CarteiraPadrao.BBCarteira11CobrancaComRegistro.ToString()*/ "",
                    VariacaoCarteiraPadrao = "019",
                    TipoCarteiraPadrao = Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = Boleto2Net.TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = Boleto2Net.TipoImpressaoBoleto.Empresa
                }
            };
        }

        private Boleto2Net.Boleto MontaBoleto(Boleto2Net.IBanco banco, ContaReceber dadosBoleto, DateTime dataDesconto, double valorDesconto)
        {
            var numerosGuidContaReceber = Regex.Replace(dadosBoleto.Id.ToString(), "[^0-9]", "");
            var randomNossoNumero = new Random().Next(0, 9999999);
            var nossoNumero = $"{randomNossoNumero.ToString("D7")}{numerosGuidContaReceber.PadLeft(10)}";

            var boleto = new Boleto2Net.Boleto(banco)
            {
                Sacado = GetSacado(dadosBoleto.Pessoa),
                DataEmissao = dadosBoleto.DataEmissao,
                DataProcessamento = DateTime.Now,
                DataVencimento = dadosBoleto.DataVencimento,
                ValorTitulo = (decimal)dadosBoleto.ValorPrevisto,
                NossoNumero = nossoNumero,
                NumeroDocumento = $"BB{randomNossoNumero.ToString("D6")}",
                EspecieDocumento = Boleto2Net.TipoEspecieDocumento.DM,
                Aceite = "N",
                CodigoInstrucao1 = "11",
                CodigoInstrucao2 = "22",
                DataDesconto = dataDesconto,
                ValorDesconto = (decimal)(dataDesconto <= DateTime.Now ? valorDesconto : 0),
                DataMulta = dadosBoleto.DataVencimento.AddDays(1),
                PercentualMulta = (decimal)percentMulta,
                ValorMulta = (decimal)(dadosBoleto.ValorPrevisto * (percentMulta / 100)),
                DataJuros = dadosBoleto.DataVencimento.AddDays(1),
                PercentualJurosDia = (decimal)jurosDia,
                ValorJurosDia = (decimal)(dadosBoleto.ValorPrevisto * (jurosDia / 100)),
                MensagemArquivoRemessa = "Mensagem para o arquivo remessa",
                NumeroControleParticipante = $"CHAVEPRIMARIA={nossoNumero}"
            };

            boleto.MensagemInstrucoesCaixa = GetInstrucoesAoCaixa(boleto);

            //boleto.Avalista = sacado;
            //boleto.Avalista.Nome = boleto.Avalista.Nome.Replace("Sacado", "Avalista");

            boleto.Demonstrativos.Add(GetDemonstrativos(boleto));

            boleto.ValidarDados();

            return boleto;
        }

        private string GetInstrucoesAoCaixa(Boleto2Net.Boleto boleto)
        {
            var msgCaixa = new StringBuilder();
            if (boleto.ValorDesconto > 0) msgCaixa.AppendLine($"Conceder desconto de {boleto.ValorDesconto.ToString("R$ ##,##0.00")} até {boleto.DataDesconto.ToString("dd/MM/yyyy")}. ");
            if (boleto.ValorMulta > 0) msgCaixa.AppendLine($"Cobrar multa de {boleto.ValorMulta.ToString("R$ ##,##0.00")} após o vencimento. ");
            if (boleto.ValorJurosDia > 0) msgCaixa.AppendLine($"Cobrar juros de {boleto.ValorJurosDia.ToString("R$ ##,##0.00")} por dia de atraso. ");

            return msgCaixa.ToString();
        }

        private Boleto2Net.GrupoDemonstrativo GetDemonstrativos(Boleto2Net.Boleto boleto)
        {
            var grupoDemonstrativo = new Boleto2Net.GrupoDemonstrativo { Descricao = "GRUPO 1" };

            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 1, Item 1", Referencia = boleto.DataEmissao.AddMonths(-1).Month + "/" + boleto.DataEmissao.AddMonths(-1).Year, Valor = boleto.ValorTitulo * (decimal)0.15 });
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 1, Item 2", Referencia = boleto.DataEmissao.AddMonths(-1).Month + "/" + boleto.DataEmissao.AddMonths(-1).Year, Valor = boleto.ValorTitulo * (decimal)0.05 });
            boleto.Demonstrativos.Add(grupoDemonstrativo);

            grupoDemonstrativo = new Boleto2Net.GrupoDemonstrativo { Descricao = "GRUPO 2" };
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 2, Item 1", Referencia = boleto.DataEmissao.Month + "/" + boleto.DataEmissao.Year, Valor = boleto.ValorTitulo * (decimal)0.20 });
            boleto.Demonstrativos.Add(grupoDemonstrativo);

            grupoDemonstrativo = new Boleto2Net.GrupoDemonstrativo { Descricao = "GRUPO 3" };
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 3, Item 1", Referencia = boleto.DataEmissao.AddMonths(-1).Month + "/" + boleto.DataEmissao.AddMonths(-1).Year, Valor = boleto.ValorTitulo * (decimal)0.37 });
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 3, Item 2", Referencia = boleto.DataEmissao.Month + "/" + boleto.DataEmissao.Year, Valor = boleto.ValorTitulo * (decimal)0.03 });
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 3, Item 3", Referencia = boleto.DataEmissao.Month + "/" + boleto.DataEmissao.Year, Valor = boleto.ValorTitulo * (decimal)0.12 });
            grupoDemonstrativo.Itens.Add(new Boleto2Net.ItemDemonstrativo { Descricao = "Grupo 3, Item 4", Referencia = boleto.DataEmissao.AddMonths(+1).Month + "/" + boleto.DataEmissao.AddMonths(+1).Year, Valor = boleto.ValorTitulo * (decimal)0.08 });

            return grupoDemonstrativo;
        }
    }
}