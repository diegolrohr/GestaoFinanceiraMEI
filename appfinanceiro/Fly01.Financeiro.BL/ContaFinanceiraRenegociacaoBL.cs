using System;
using System.Linq;
using System.Data.Entity;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ContaFinanceiraRenegociacaoBL : EmpresaBaseBL<ContaFinanceiraRenegociacao>
    {
        private RenegociacaoContaFinanceiraOrigemBL renegociacaoContaFinanceiraOrigemBL;
        private RenegociacaoContaFinanceiraRenegociadaBL renegociacaoContaFinanceiraRenegociadaBL;
        private ContaPagarBL contaPagarBL;
        private ContaReceberBL contaReceberBL;

        public ContaFinanceiraRenegociacaoBL(AppDataContext context, RenegociacaoContaFinanceiraOrigemBL renegociacaoContaFinanceiraOrigemBL, RenegociacaoContaFinanceiraRenegociadaBL renegociacaoContaFinanceiraRenegociadaBL, ContaPagarBL contaPagarBL, ContaReceberBL contaReceberBL)
            : base(context)
        {
            this.renegociacaoContaFinanceiraOrigemBL = renegociacaoContaFinanceiraOrigemBL;
            this.renegociacaoContaFinanceiraRenegociadaBL = renegociacaoContaFinanceiraRenegociadaBL;
            this.contaPagarBL = contaPagarBL;
            this.contaReceberBL = contaReceberBL;
        }

        public override void ValidaModel(ContaFinanceiraRenegociacao renegociacao)
        {
            renegociacao.Fail(!renegociacao.ContasFinanceirasOrigemIds.Any(), SelecioneContasOrigem);
            renegociacao.Fail(renegociacao.ValorDiferenca < 0, ValorDiferencaInvalido);

            if (renegociacao.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
            {
                var contasOrigem = contaPagarBL.All.Where(x => renegociacao.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList();

                if (contasOrigem.Any())
                {
                    contasOrigem.ForEach(item =>
                    {
                        if (item.PessoaId != renegociacao.PessoaId)
                            throw new BusinessException("Adicione somente contas financeiras da mesma pessoa, informada na renegociação.");

                        if (item.StatusContaBancaria != StatusContaBancaria.EmAberto && item.StatusContaBancaria != StatusContaBancaria.BaixadoParcialmente && item.StatusContaBancaria != StatusContaBancaria.Renegociado)
                            throw new BusinessException("Adicione somente contas financeiras de status (Em aberto), (Baixado parcialmente) ou (Renegociada).");
                    });
                }
                else
                    throw new BusinessException("Contas de origem do tipo a Pagar, não encontradas. Adicione somente contas financeiras do mesmo tipo desta renegociação.");
            }
            else if (renegociacao.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
            {
                var contasOrigem = contaReceberBL.All.Where(x => renegociacao.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList();

                if (contasOrigem.Any())
                {
                    contasOrigem.ForEach(item =>
                    {
                        if (item.PessoaId != renegociacao.PessoaId)
                            throw new BusinessException("Adicione somente contas financeiras da mesma pessoa, informada na renegociação.");

                        if (item.StatusContaBancaria != StatusContaBancaria.EmAberto && item.StatusContaBancaria != StatusContaBancaria.BaixadoParcialmente && item.StatusContaBancaria != StatusContaBancaria.Renegociado)
                            throw new BusinessException("Adicione somente contas financeiras de status (Em aberto), (Baixado parcialmente) ou (Renegociada).");
                    });
                }
                else
                    throw new BusinessException("Contas de origem do tipo a Receber, não encontradas. Adicione somente contas financeiras do mesmo tipo desta renegociação.");
            }
            else
                throw new BusinessException("Renegociação somente do tipo contas a Pagar/Receber.");

            base.ValidaModel(renegociacao);
        }

        private double CalculaValorAcumulado(ContaFinanceiraRenegociacao entity)
        {
            double soma = 0.00;
            if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
            {
                var contasOrigem = contaPagarBL.All.Where(x => entity.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList();
                contasOrigem.ForEach(item =>
                {
                    soma += item.Saldo;
                });
            }
            else if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
            {
                var contasOrigem = contaReceberBL.All.Where(x => entity.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList();
                contasOrigem.ForEach(item =>
                {
                    soma += item.Saldo;
                });
            }
            return soma;
        }

        private void CalculaValorFinal(ContaFinanceiraRenegociacao entity)
        {
            double valorDiferencaCalculado = 0.00;
            if (entity.TipoRenegociacaoCalculo == TipoRenegociacaoCalculo.Valor)
                valorDiferencaCalculado = entity.ValorDiferenca;
            else if (entity.TipoRenegociacaoCalculo == TipoRenegociacaoCalculo.Percentual)
                valorDiferencaCalculado = ((entity.ValorAcumulado * entity.ValorDiferenca) / 100.00);
            else
                throw new BusinessException("Tipo do cálculo da diferença inválido.");

            if (entity.TipoRenegociacaoValorDiferenca == TipoRenegociacaoValorDiferenca.Acrescimo)
            {
                var valorFinal = (entity.ValorAcumulado + valorDiferencaCalculado);

                if (valorFinal <= 0)
                    throw new BusinessException("Valor final deve ser superior a zero.");
                else
                    entity.ValorFinal = valorFinal;
            }
            else if (entity.TipoRenegociacaoValorDiferenca == TipoRenegociacaoValorDiferenca.Desconto)
            {
                var valorFinal = (entity.ValorAcumulado - valorDiferencaCalculado);

                if (valorFinal <= 0)
                    throw new BusinessException("Valor final deve ser superior a zero.");
                else
                    entity.ValorFinal = valorFinal;
            }
            else
                throw new BusinessException("Tipo do valor da diferença inválido.");
        }

        private void SalvaRelacionamentoContasOrigem(ContaFinanceiraRenegociacao entity)
        {
            var contasFinanceirasOrigem = entity.ContasFinanceirasOrigemIds.Select(
                x => new RenegociacaoContaFinanceiraOrigem
                {
                    ContaFinanceiraRenegociacaoId = entity.Id,
                    ContaFinanceiraId = x
                }
            ).ToList();

            foreach (var item in contasFinanceirasOrigem)
            {
                renegociacaoContaFinanceiraOrigemBL.Insert(item);
            }
        }

        private void AtualizaStatusContasOrigem(ContaFinanceiraRenegociacao entity)
        {
            if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
            {
                contaPagarBL.All.Where(x => entity.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList().ForEach(item =>
                {
                    item.StatusContaBancaria = StatusContaBancaria.Renegociado;
                });
            }
            else if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
            {
                contaReceberBL.All.Where(x => entity.ContasFinanceirasOrigemIds.Contains(x.Id)).ToList().ForEach(item =>
                {
                    item.StatusContaBancaria = StatusContaBancaria.Renegociado;
                });
            }
        }

        private void InsertNovaContaRenegociada(ContaFinanceiraRenegociacao entity)
        {
            if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaPagar)
            {
                contaPagarBL.Insert(new ContaPagar()
                {
                    CategoriaId = entity.CategoriaId,
                    CondicaoParcelamentoId = entity.CondicaoParcelamentoId,
                    DataEmissao = entity.DataEmissao,
                    DataVencimento = entity.DataVencimento,
                    Descricao = entity.Descricao,
                    FormaPagamentoId = entity.FormaPagamentoId,
                    PessoaId = entity.PessoaId,
                    Repetir = false,
                    TipoPeriodicidade = TipoPeriodicidade.Nenhuma,
                    StatusContaBancaria = StatusContaBancaria.EmAberto,
                    ValorPrevisto = entity.ValorFinal
                });
            }
            else if (entity.TipoContaFinanceira == TipoContaFinanceira.ContaReceber)
            {
                contaReceberBL.Insert(new ContaReceber()
                {
                    CategoriaId = entity.CategoriaId,
                    CondicaoParcelamentoId = entity.CondicaoParcelamentoId,
                    DataEmissao = entity.DataEmissao,
                    DataVencimento = entity.DataVencimento,
                    Descricao = entity.Descricao,
                    FormaPagamentoId = entity.FormaPagamentoId,
                    PessoaId = entity.PessoaId,
                    Repetir = false,
                    TipoPeriodicidade = TipoPeriodicidade.Nenhuma,
                    StatusContaBancaria = StatusContaBancaria.EmAberto,
                    ValorPrevisto = entity.ValorFinal
                });
            }
        }

        private void SalvaRelacionamentoContasRenegociadas(ContaFinanceiraRenegociacao entity)
        {
            string tipoConta = entity.TipoContaFinanceira.ToString();

            var contasRenegociadasInsert = repository.GetAffectEntries()
                                            .Where(x => x.State == EntityState.Added && x.Entity.GetType().Name == tipoConta)
                                            .Select(x => x.CurrentValues.GetValue<object>("Id").ToString()).ToList();

            var contasFinanceirasRenegociadas = contasRenegociadasInsert.Select(
                x => new RenegociacaoContaFinanceiraRenegociada
                {
                    ContaFinanceiraRenegociacaoId = entity.Id,
                    ContaFinanceiraId = Guid.Parse(x)
                }
            ).ToList();

            foreach (var item in contasFinanceirasRenegociadas)
            {
                renegociacaoContaFinanceiraRenegociadaBL.Insert(item);
            }
        }

        public override void Insert(ContaFinanceiraRenegociacao entity)
        {
            try
            {
                ValidaModel(entity);

                entity.Id = Guid.NewGuid();
                entity.ValorAcumulado = CalculaValorAcumulado(entity);

                CalculaValorFinal(entity);
                SalvaRelacionamentoContasOrigem(entity);
                AtualizaStatusContasOrigem(entity);
                InsertNovaContaRenegociada(entity);
                SalvaRelacionamentoContasRenegociadas(entity);

                base.Insert(entity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public override void Update(ContaFinanceiraRenegociacao entity)
        {
            throw new BusinessException("Não é possível alterar uma renegociação.");
        }

        public override void Delete(ContaFinanceiraRenegociacao entity)
        {
            throw new BusinessException("Não é possível excluir uma renegociação.");
        }

        public static Error SelecioneContasOrigem = new Error("Selecione as contas de origem.");
        public static Error ValorDiferencaInvalido = new Error("Valor da diferença inválido.");
    }
}