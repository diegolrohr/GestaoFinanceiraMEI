﻿using System;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Linq;
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

        private string codigoCedente;
        const double jurosDia = 0.33;
        const double percentMulta = 2.00;

        public CnabBL(AppDataContextBase context, ContaReceberBL contaReceberBL, ContaBancariaBL contaBancariaBL) : base(context)
        {
            this.contaReceberBL = contaReceberBL;
            this.contaBancariaBL = contaBancariaBL;
        }

        public Cnab GetCnab(Guid Id)
        {
            var cnab = base.AllIncluding(b => b.ContaReceber, b => b.ContaReceber.Pessoa);

            if (Id != Guid.Empty)
                return cnab.Where(x => x.Id == Id).FirstOrDefault();

            return cnab.FirstOrDefault();
        }

        public BoletoVM GetDadosBoleto(Guid contaReceberId, Guid contaBancariaId)
        {
            var contaReceber = contaReceberBL.Find(contaReceberId);
            var contaBancariaCedente = contaBancariaBL.AllIncluding(r => r.Banco).FirstOrDefault(x => x.Id == contaBancariaId);
            var cedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);
            var sacado = contaReceberBL.AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado).Where(x => x.PessoaId == contaReceber.PessoaId).FirstOrDefault()?.Pessoa;
            //codigoCedente = "1234657";
            codigoCedente = "123465";

            var valorMulta = (decimal)(contaReceber.ValorPrevisto * (percentMulta / 100));
            var valorJuros = (decimal)(contaReceber.ValorPrevisto * (jurosDia / 100));
            var numerosGuidContaReceber = Regex.Replace(contaReceber.Id.ToString(), "[^0-9]", "");
            var randomNossoNumero = new Random().Next(0, 9999999);
            var nossoNumero = $"{codigoCedente}{numerosGuidContaReceber.Substring(0, Math.Min(10, numerosGuidContaReceber.Length)).PadLeft(10, '0')}";

            var boletoVM = new BoletoVM()
            {
                ValorPrevisto = (decimal)contaReceber.ValorPrevisto,
                ValorDesconto = contaReceber.ValorDesconto.HasValue ? (decimal)contaReceber.ValorDesconto : 0,
                ValorMulta = valorMulta,
                ValorJuros = valorJuros,
                DataEmissao = DateTime.Now,
                DataDesconto = contaReceber.DataDesconto ?? null,
                DataVencimento = contaReceber.DataVencimento,
                NossoNumero = nossoNumero,
                EspecieMoeda = "R$",
                NumeroDocumento = $"BB{randomNossoNumero.ToString("D6")}",
                InstrucoesCaixa = GetInstrucoesAoCaixa(contaReceber),
                Cedente = GetDadosCedente(contaBancariaId),
                Sacado = GetDadosSacado(contaReceber.PessoaId)
            };

            if (ValidaDadosBoleto(boletoVM))
                return boletoVM;
            else
                return null;
        }

        private bool ValidaDadosBoleto(BoletoVM boleto)
        {
            if (string.IsNullOrEmpty(boleto.Cedente.EnderecoComplemento)) boleto.Cedente.EnderecoComplemento = "---";
            if (string.IsNullOrEmpty(boleto.Sacado.EnderecoComplemento)) boleto.Sacado.EnderecoComplemento = "---";
            
            return true;
        }

        private SacadoVM GetDadosSacado(Guid pessoaId)
        {
            var sacado = contaReceberBL.AllIncluding(r => r.Pessoa, r => r.Pessoa.Cidade, r => r.Pessoa.Cidade.Estado).Where(x => x.PessoaId == pessoaId).FirstOrDefault()?.Pessoa;

            return new SacadoVM
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
            };
        }

        private CedenteVM GetDadosCedente(Guid contaBancariaId)
        {
            var cedente = ApiEmpresaManager.GetEmpresa(PlataformaUrl);

            return new CedenteVM
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
                ContaBancariaCedente = GetContaBancariaCedente(contaBancariaId)
            };
        }

        private ContaBancariaCedenteVM GetContaBancariaCedente(Guid contaBancariaId)
        {
            var contaBancariaCedente = contaBancariaBL.AllIncluding(b => b.Banco).FirstOrDefault(x => x.Id == contaBancariaId);

            return new ContaBancariaCedenteVM
            {
                Agencia = contaBancariaCedente.Agencia,
                DigitoAgencia = contaBancariaCedente.DigitoAgencia,
                Conta = contaBancariaCedente.Conta,
                DigitoConta = contaBancariaCedente.DigitoConta,
                CodigoBanco = int.Parse(contaBancariaCedente.Banco.Codigo)
            };
        }

        private string GetInstrucoesAoCaixa(ContaReceber conta)
        {
            var valorMulta = (decimal)(conta.ValorPrevisto * (percentMulta / 100));
            var valorJuros = (decimal)(conta.ValorPrevisto * (jurosDia / 100));
            var msgCaixa = new StringBuilder();

            if (conta.ValorDesconto.HasValue && conta.DataDesconto.HasValue) msgCaixa.AppendLine($"Conceder desconto de {conta.ValorDesconto.Value.ToString("R$ ##,##0.00")} até {conta.DataDesconto.Value.ToString("dd/MM/yyyy")}. ");
            if (percentMulta > 0) msgCaixa.AppendLine($"Cobrar multa de {valorMulta.ToString("R$ ##,##0.00")} após o vencimento. ");
            if (jurosDia > 0) msgCaixa.AppendLine($"Cobrar juros de {valorJuros.ToString("R$ ##,##0.00")} por dia de atraso. ");

            return msgCaixa.ToString();
        }
    }
}