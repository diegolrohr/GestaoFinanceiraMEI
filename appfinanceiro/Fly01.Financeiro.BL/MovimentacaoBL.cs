using Fly01.Core.BL;
using System;
using System.Linq;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class MovimentacaoBL : PlataformaBaseBL<MovimentacaoFinanceira>
    {
        private CategoriaBL categoriaBL;
        private SaldoHistoricoBL saldoHistoricoBL;

        public MovimentacaoBL(AppDataContext context, CategoriaBL categoriaBL, SaldoHistoricoBL saldoHistoricoBL) : base(context)
        {
            this.categoriaBL = categoriaBL;
            this.saldoHistoricoBL = saldoHistoricoBL;
        }

        public override void Insert(MovimentacaoFinanceira movimentacao)
        {
            movimentacao.Fail(movimentacao.ContaBancariaOrigemId == default(Guid) && movimentacao.ContaBancariaDestinoId == default(Guid), new Error("Conta Bancária Inválida."));
            movimentacao.Fail(movimentacao.CategoriaId == null, new Error("Informe a categoria financeira."));
            movimentacao.Fail(movimentacao.Data.Date > DateTime.Now.Date, new Error("Data da transação não pode ser superior a data atual"));

            var categoria = categoriaBL.All.FirstOrDefault(x => x.Id == movimentacao.CategoriaId);

            if (categoria != null)
            {
                //movimentacao.Fail(categoria.Classe != CategoriaFinanceiraClasse.Analitico, new Error("A categoria financeira deve ser do tipo de classe analítica."));

                if (string.IsNullOrWhiteSpace(movimentacao.Descricao))
                    movimentacao.Descricao = categoria.Descricao;
            }

            movimentacao.UsuarioInclusao = AppUser;
            movimentacao.PlataformaId = PlataformaUrl;
            movimentacao.DataInclusao = DateTime.Now;
            movimentacao.Ativo = true;
            movimentacao.Descricao = movimentacao.Descricao.Substring(0, movimentacao.Descricao.Length > 200 ? 200 : movimentacao.Descricao.Length);

            base.ValidaModel(movimentacao);

            switch (categoria.TipoCarteira)
            {
                case TipoCarteira.Despesa:
                    Debito(movimentacao.Data, movimentacao.Valor, movimentacao.ContaBancariaOrigemId.Value, movimentacao.ContaFinanceiraId, movimentacao.Descricao, movimentacao.CategoriaId);// .CopyProperties<Movimentacao>(movimentacao);
                    saldoHistoricoBL.AtualizaSaldoHistorico(movimentacao.Data, (movimentacao.Valor), movimentacao.ContaBancariaOrigemId.Value, TipoCarteira.Despesa);
                    break;
                case TipoCarteira.Receita:
                    Credito(movimentacao.Data, movimentacao.Valor, movimentacao.ContaBancariaDestinoId.Value, movimentacao.ContaFinanceiraId, movimentacao.Descricao, movimentacao.CategoriaId); //.CopyProperties<Movimentacao>(movimentacao);
                    saldoHistoricoBL.AtualizaSaldoHistorico(movimentacao.Data, movimentacao.Valor, movimentacao.ContaBancariaDestinoId.Value, TipoCarteira.Receita);
                    break;
            }
        }

        public override void Update(MovimentacaoFinanceira entity)
        {
            throw new BusinessException("Não é possivel realizar a atualização de movimentação.");
        }

        public override void Delete(MovimentacaoFinanceira entityToDelete)
        {
            throw new BusinessException("Não é possivel realizar a deleção de movimentação.");
        }

        internal void CriaMovimentacao(DateTime dataMovimento, double valor, Guid contaBancariaId, TipoContaFinanceira tipoContaFinanceira, Guid contaFinanceiraId, string descricao = null)
        {
            switch (tipoContaFinanceira)
            {
                case TipoContaFinanceira.ContaPagar:
                    Debito(dataMovimento, valor, contaBancariaId, contaFinanceiraId, descricao, null);
                    break;
                case TipoContaFinanceira.ContaReceber:
                    Credito(dataMovimento, valor, contaBancariaId, contaFinanceiraId, descricao, null);
                    break;
                default:
                    throw new NotImplementedException("Erro ao criar movimentação financeira. O TipoContaFinanceira informado não é válido!");
            }
        }

        public void NovaTransferencia(TransferenciaFinanceira transferencia)
        {
            transferencia.PlataformaId = PlataformaUrl;
            transferencia.UsuarioInclusao = AppUser;

            var categoriaOrigem = categoriaBL
                                    .All
                                    .FirstOrDefault(x => x.Id == transferencia.MovimentacaoOrigem.CategoriaId);

            if (categoriaOrigem?.TipoCarteira != TipoCarteira.Despesa)
            {
                throw new BusinessException("A categoria financeira origem dever ser do tipo de carteira despesa.");
            }

            var categoriaDestino = categoriaBL
                                    .All
                                    .FirstOrDefault(x => x.Id == transferencia.MovimentacaoDestino.CategoriaId);

            if (categoriaDestino?.TipoCarteira != TipoCarteira.Receita)
            {
                throw new BusinessException("A categoria financeira destino dever ser do tipo de carteira receita.");
            }

            if (transferencia.MovimentacaoOrigem.ContaBancariaOrigemId == null || transferencia.MovimentacaoDestino.ContaBancariaDestinoId == null)
            {
                throw new BusinessException("Informe a conta bancária de origem na movimentação de origem e a conta bancária de destino na movimentação de destino.");
            }

            if (!Math.Abs(transferencia.MovimentacaoOrigem.Valor).Equals(Math.Abs(transferencia.MovimentacaoDestino.Valor)))
            {
                throw new BusinessException("O mesmo valor que será debitado de uma conta origem, deverá ser credidato em uma conta destino.");
            }

            if (!(transferencia.MovimentacaoOrigem.Valor < 0) || !(transferencia.MovimentacaoDestino.Valor > 0))
            {
                throw new BusinessException("O valor de origem deve ser negativo e o valor de destino deve ser positivo.");
            }

            if (transferencia.MovimentacaoOrigem.ContaBancariaOrigemId == null || transferencia.MovimentacaoOrigem.ContaBancariaDestinoId != null)
            {
                throw new BusinessException("A conta bancária de origem na movimentação de origem deve ser preenchida e a conta bancária de destino deve estar nula.");
            }

            if (transferencia.MovimentacaoDestino.ContaBancariaDestinoId == null || transferencia.MovimentacaoDestino.ContaBancariaOrigemId != null)
            {
                throw new BusinessException("A conta bancária de destino na movimentação de destino deve ser preenchida e a conta bancária de origem deve estar nula.");
            }

            if (transferencia.MovimentacaoDestino.ContaBancariaDestinoId == transferencia.MovimentacaoDestino.ContaBancariaOrigemId)
            {
                throw new BusinessException("A conta bancária de destino de origem não podem ser a mesma.");
            }

            transferencia.MovimentacaoOrigem.Valor *= -1;

            Insert(transferencia.MovimentacaoOrigem);
            Insert(transferencia.MovimentacaoDestino);
        }

        private MovimentacaoFinanceira Debito(DateTime data, double valor, Guid contaBancariaId, Guid? contaFinanceiraId = null, string descricao = null , Guid? categoriaId = null)
        {
            var mov = new MovimentacaoFinanceira()
            {
                ContaBancariaDestinoId = new Guid?(),
                ContaBancariaOrigemId = contaBancariaId,
                ContaFinanceiraId = contaFinanceiraId,
                CategoriaId = categoriaId,
                Data = data,
                Valor = valor * -1,
                Descricao = descricao
            };
            base.Insert(mov);

            return mov;
        }

        private MovimentacaoFinanceira Credito(DateTime data, double valor, Guid contaBancariaId, Guid? contaFinanceiraId = null, string descricao = null, Guid? categoriaId = null)
        {
            var mov = new MovimentacaoFinanceira()
            {
                ContaBancariaDestinoId = contaBancariaId,
                ContaBancariaOrigemId = new Guid?(),
                ContaFinanceiraId = contaFinanceiraId,
                CategoriaId = categoriaId,
                Data = data,
                Valor = valor,
                Descricao = descricao
            };
            base.Insert(mov);

            return mov;
        }
    }
}