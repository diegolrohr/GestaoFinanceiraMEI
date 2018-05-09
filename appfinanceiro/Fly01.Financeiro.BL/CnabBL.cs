﻿using System;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
using System.Collections.Generic;
using Fly01.Core.ViewModels;
using System.Text.RegularExpressions;
using System.Text;
using Fly01.Core.Rest;

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

        private string GetInstrucoesAoCaixa(ContaReceber conta, DateTime dataDesconto, double valorDesconto)
        {
            var valorMulta = (decimal)(conta.ValorPrevisto * (percentMulta / 100));
            var valorJuros = (decimal)(conta.ValorPrevisto * (jurosDia / 100));
            var msgCaixa = new StringBuilder();

            if (valorDesconto > 0) msgCaixa.AppendLine($"Conceder desconto de {valorDesconto.ToString("R$ ##,##0.00")} até {dataDesconto.ToString("dd/MM/yyyy")}. ");
            if (percentMulta > 0) msgCaixa.AppendLine($"Cobrar multa de {valorMulta.ToString("R$ ##,##0.00")} após o vencimento. ");
            if (jurosDia > 0) msgCaixa.AppendLine($"Cobrar juros de {valorJuros.ToString("R$ ##,##0.00")} por dia de atraso. ");

            return msgCaixa.ToString();
        }

        public List<Cnab> Get()
        {
            return base.All.ToList();
        }

        public BoletoVM GetDadosBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            var contaReceber = contaReceberBL.Find(contaReceberId);
            var contaBancariaCedente = contaBancariaBL.Find(contaBancariaId);
            var banco = contaBancariaBL.AllIncluding(b => b.Banco).Where(x => x.BancoId == contaBancariaCedente.BancoId).FirstOrDefault();
            var cedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var sacado = contaReceberBL.AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado).Where(x => x.PessoaId == contaReceber.PessoaId).FirstOrDefault()?.Pessoa;
            codigoCedente = "1234657";

            var valorMulta = (decimal)(contaReceber.ValorPrevisto * (percentMulta / 100));
            var valorJuros = (decimal)(contaReceber.ValorPrevisto * (jurosDia / 100));
            var numerosGuidContaReceber = Regex.Replace(contaReceber.Id.ToString(), "[^0-9]", "");
            var randomNossoNumero = new Random().Next(0, 9999999);
            var nossoNumero = $"{codigoCedente}{numerosGuidContaReceber.Substring(0, Math.Min(10, numerosGuidContaReceber.Length)).PadLeft(10, '0')}";

            return new BoletoVM()
            {
                ValorPrevisto = (decimal)contaReceber.ValorPrevisto,
                ValorDesconto = (decimal)0,
                ValorMulta = valorMulta,
                ValorJuros = valorJuros,
                DataEmissao = DateTime.Now,
                DataVencimento = contaReceber.DataVencimento,
                NossoNumero = nossoNumero,
                EspecieMoeda = "R$",
                NumeroDocumento = $"BB{randomNossoNumero.ToString("D6")}",
                InstrucoesCaixa = GetInstrucoesAoCaixa(contaReceber, DateTime.Now, 0),
                Cedente = new CedenteVM
                {
                    CNPJ = cedente.CNPJ,
                    RazaoSocial = cedente.RazaoSocial,
                    Endereco = cedente.Endereco,
                    EnderecoNumero = cedente.Numero,
                    EnderecoComplemento = "",
                    EnderecoBairro = cedente.Bairro,
                    EnderecoCidade = cedente.Cidade.Nome,
                    EnderecoUF = cedente.Cidade.Estado.Sigla,
                    EnderecoCEP = cedente.CEP,
                    Observacoes = "",
                    CodigoCedente = codigoCedente,
                    ContaBancariaCedente = new ContaBancariaCedenteVM
                    {
                        Agencia = contaBancariaCedente.Agencia,
                        DigitoAgencia = contaBancariaCedente.DigitoAgencia,
                        Conta = contaBancariaCedente.Conta,
                        DigitoConta = contaBancariaCedente.DigitoConta,
                        CodigoBanco = int.Parse(contaBancariaCedente.Banco.Codigo)
                    }
                },
                Sacado = new SacadoVM
                {
                    CNPJ = sacado.CPFCNPJ,
                    Nome = sacado.Nome,
                    Endereco = sacado.Endereco,
                    EnderecoNumero = sacado.Numero,
                    EnderecoComplemento = sacado.Complemento,
                    EnderecoBairro = sacado.Bairro,
                    EnderecoCidade = sacado.Cidade?.Nome,
                    EnderecoUF = sacado.Cidade?.Estado?.Sigla,
                    EnderecoCEP = sacado.CEP,
                    Observacoes = ""
                }
            };
        }
    }
}