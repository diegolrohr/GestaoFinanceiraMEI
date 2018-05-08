using System;
using System.Text;
using Fly01.Core.BL;
using Fly01.Core.Rest;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Text.RegularExpressions;
using System.Linq;
using Fly01.Financeiro.Controllers;

namespace Fly01.Financeiro.BL
{
    public class CnabBL : PlataformaBaseBL<Cnab>
    {
        protected ContaReceberBL contaReceberBL;
        protected ContaBancariaBL contaBancariaBL;

        private Boleto2Net.Boletos boletos;
        private string codigoCedente;
        const double jurosDia = 0.33;
        const double percentMulta = 2.00;

        public CnabBL(AppDataContextBase context, ContaReceberBL contaReceberBL, ContaBancariaBL contaBancariaBL) : base(context)
        {
            boletos = new Boleto2Net.Boletos();
            this.contaReceberBL = contaReceberBL;
            this.contaBancariaBL = contaBancariaBL;
        }

        public BoletoBanco GeraBoletos(Guid contaReceberId, Guid contaBancariaId, DateTime dataDesconto, double valorDesconto)
        {
            var contaReceber = contaReceberBL.Find(contaReceberId);
            if (contaReceber == null) throw new BusinessException("A conta a receber informada não foi encontrada.");

            var contaBancaria = contaBancariaBL.Find(contaBancariaId);
            if (contaBancaria == null) throw new BusinessException("A conta bancária informada não foi encontrada.");

            var banco = contaBancariaBL.AllIncluding(b => b.Banco).Where(x => x.BancoId == contaBancaria.BancoId).FirstOrDefault();
            if (!banco.Banco.EmiteBoleto) throw new BusinessException("Não é possível emitir boletos para esta instituição bancária.");

            var bancoCedente = Boleto2Net.Banco.Instancia(ushort.Parse(contaBancaria.Banco.Codigo));

            bancoCedente.Cedente = GetCedenteBoletoNet(contaBancaria);
            bancoCedente.FormataCedente();

            return MontaBoleto(bancoCedente, contaReceber, dataDesconto, valorDesconto);
        }

        private Boleto2Net.Sacado GetSacado(Guid pessoaId)
        {
            var pessoa = contaReceberBL
                .AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado)
                .Where(x => x.PessoaId == pessoaId).FirstOrDefault()?.Pessoa;

            return new Boleto2Net.Sacado
            {
                CPFCNPJ = pessoa.CPFCNPJ,
                Nome = pessoa.Nome,
                Observacoes = "",
                Endereco = new Boleto2Net.Endereco
                {
                    LogradouroEndereco = pessoa.Endereco,
                    LogradouroNumero = pessoa.Numero,
                    Bairro = pessoa.Bairro,
                    Cidade = pessoa.Cidade?.Nome,
                    UF = pessoa.Cidade?.Estado?.Sigla,
                    CEP = pessoa.CEP
                }
            };
        }

        private Boleto2Net.Cedente GetCedenteBoletoNet(ContaBancaria contaCedente)
        {
            var dadosEmpresaCedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            codigoCedente = "1234657";

            return new Boleto2Net.Cedente
            {
                CPFCNPJ = dadosEmpresaCedente.CNPJ,
                Nome = dadosEmpresaCedente.NomeFantasia,
                Codigo = codigoCedente,
                CodigoDV = "",
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
                    CarteiraPadrao = "11", //contaCedente.CarteiraPadrao,
                    VariacaoCarteiraPadrao = "019", //contaCedente.VariacaoCarteiraPadrao,
                    TipoCarteiraPadrao = Boleto2Net.TipoCarteira.CarteiraCobrancaSimples,
                    TipoFormaCadastramento = Boleto2Net.TipoFormaCadastramento.ComRegistro,
                    TipoImpressaoBoleto = Boleto2Net.TipoImpressaoBoleto.Empresa
                }
            };
        }

        private BoletoBanco MontaBoleto(Boleto2Net.IBanco banco, ContaReceber dadosBoleto, DateTime dataDesconto, double valorDesconto)
        {
            var numerosGuidContaReceber = Regex.Replace(dadosBoleto.Id.ToString(), "[^0-9]", "");
            var randomNossoNumero = new Random().Next(0, 9999999);
            var nossoNumero = $"{codigoCedente}{numerosGuidContaReceber.Substring(0, Math.Min(10, numerosGuidContaReceber.Length)).PadLeft(10, '0')}";

            var boleto = new Boleto2Net.Boleto(banco)
            {
                Sacado = GetSacado(dadosBoleto.PessoaId),
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

            boleto.Avalista = new Boleto2Net.Sacado();
            boleto.Avalista.Nome = boleto.Avalista.Nome.Replace("Sacado", "Avalista");

            boleto.Demonstrativos.Add(GetDemonstrativos(boleto));

            boleto.ValidarDados();

            return new BoletoBanco(boleto.Banco.Codigo)
            {
                MeuBoleto = boleto
            };

            //return BoletoBanco()
            //{
            //    CodigoBanco = boleto.Banco.Codigo,
            //    Boleto = boleto
            //};
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