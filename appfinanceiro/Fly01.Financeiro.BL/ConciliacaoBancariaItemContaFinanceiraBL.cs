using System.Data.Entity;
using Fly01.Core.BL;
using System.Linq;
using System;
using System.Collections.Generic;
using Fly01.Financeiro.API.Models.DAL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using Fly01.Core.Entities.Domains.Enum;

namespace Fly01.Financeiro.BL
{
    public class ConciliacaoBancariaItemContaFinanceiraBL : EmpresaBaseBL<ConciliacaoBancariaItemContaFinanceira>
    {
        protected ConciliacaoBancariaBL conciliacaoBancariaBL { get; set; }
        protected ConciliacaoBancariaItemBL conciliacaoBancariaItemBL { get; set; }
        protected ContaFinanceiraBaixaBL contaFinanceiraBaixaBL { get; set; }
        protected ContaFinanceiraBL contaFinanceiraBL { get; set; }

        public ConciliacaoBancariaItemContaFinanceiraBL(AppDataContext context, ConciliacaoBancariaBL ConciliacaoBancariaBL, ConciliacaoBancariaItemBL ConciliacaoBancariaItemBL, ContaFinanceiraBaixaBL ContaFinanceiraBaixaBL, ContaFinanceiraBL ContaFinanceiraBL)
            : base(context)
        {
            this.conciliacaoBancariaBL = ConciliacaoBancariaBL;
            this.conciliacaoBancariaItemBL = ConciliacaoBancariaItemBL;
            this.contaFinanceiraBaixaBL = ContaFinanceiraBaixaBL;
            this.contaFinanceiraBL = ContaFinanceiraBL;
        }

        private void ValidaModel(ConciliacaoBancariaItemContaFinanceira entity, ConciliacaoBancariaItem conciliacaoBancariaItem, double somaValorConciliadoExistentes = 0)
        {
            if (entity.ValorConciliado <= 0)
            {
                throw new BusinessException("Valor conciliado deve ser superior a zero");
            }

            var contaFinanceira = entity.ContaFinanceiraId != default(Guid) ? contaFinanceiraBL.All.FirstOrDefault(x => x.Id == entity.ContaFinanceiraId) : entity.ContaFinanceira;
            if (contaFinanceira == null)
            {
                throw new BusinessException("Conta financeira inválida");
            }

            if(entity.ValorConciliado > contaFinanceira.Saldo)
            {
                throw new BusinessException("Valor conciliado não pode ser superior ao saldo da conta financeira");
            }

            entity.ContaFinanceiraId = contaFinanceira.Id;

            if (conciliacaoBancariaItem.Valor < 0)
            {
                if (contaFinanceira.TipoContaFinanceira != TipoContaFinanceira.ContaPagar)
                {
                    throw new BusinessException("Lançamento negativo só pode ser vinculado a contas financeiras do tipo a pagar");
                }
            }
            else
            {
                if (contaFinanceira.TipoContaFinanceira != TipoContaFinanceira.ContaReceber)
                {
                    throw new BusinessException("Lançamento positivo só pode ser vinculado a contas financeiras do tipo a receber");
                }
            }

            //validação da contaFinanceiraId, já existe na baixa, não reescrevi aqui
            var items = All.Where(x => x.ConciliacaoBancariaItemId == entity.ConciliacaoBancariaItemId);
            double somaValorJaConciliado = 0;
            //somaValorConciliadoExistentes quando é insert de um lista resultante do buscar existentes, não estão salvos ainda
            //não estão no all. porém cada um será salvo e precisa ser validado
            somaValorJaConciliado += somaValorConciliadoExistentes;
            if (items.Any())
              somaValorJaConciliado = Math.Round(items.Sum(y => y.ValorConciliado),2);

            var somaConciliado = Math.Round((somaValorJaConciliado + entity.ValorConciliado), 2);
            var valor = Math.Round(Math.Abs(conciliacaoBancariaItem.Valor), 2);
            if (somaConciliado > valor)
            {
                throw new BusinessException("A soma dos valores conciliados não pode ser superior ao valor do lançamento");
            }
            if (somaConciliado == valor)
            {
                conciliacaoBancariaItem.StatusConciliado = StatusConciliado.Sim;
            }
            else
            {
                conciliacaoBancariaItem.StatusConciliado = StatusConciliado.Parcial;
            }
        }

        private void SalvaContaFinanceiraBaixa(ConciliacaoBancariaItemContaFinanceira entity, ConciliacaoBancariaItem conciliacaoBancariaItem)
        {
            //pelo tipo da conta financeira vinculada, realiza a baixa de acordo
            var contaFinanceiraBaixaId = Guid.NewGuid();
            entity.ContaFinanceiraBaixaId = contaFinanceiraBaixaId;

            contaFinanceiraBaixaBL.Insert(new ContaFinanceiraBaixa()
            {
                Id = contaFinanceiraBaixaId,
                Ativo = true,
                ContaBancariaId = conciliacaoBancariaBL.All.FirstOrDefault(x => x.Id == conciliacaoBancariaItem.ConciliacaoBancariaId).ContaBancariaId,
                ContaFinanceiraId = entity.ContaFinanceira != null ? default(Guid) : entity.ContaFinanceiraId,
                ContaFinanceira = entity.ContaFinanceira != null ? entity.ContaFinanceira : null,
                Data = conciliacaoBancariaItem.Data.Date,
                Valor = entity.ValorConciliado
            });
        }

        private List<ConciliacaoBancariaItemContaFinanceira> BuscaSugestoesPorItem(ConciliacaoBancariaItem lancamento)
        {
            var tipoContaFinanceira = lancamento.Valor < 0 ? TipoContaFinanceira.ContaPagar : TipoContaFinanceira.ContaReceber;
            var dataIni = lancamento.Data.AddDays(-5.0);
            var valor = Math.Abs(lancamento.Valor);

            var sugestoes = contaFinanceiraBL.All.Where(
                x => x.TipoContaFinanceira == tipoContaFinanceira &&
                ((x.ValorPago == default(double)) || (x.ValorPago == null)) && //que não possua nenhuma baixa
                x.ValorPrevisto == valor &&
                ((x.DataVencimento >= dataIni) && (x.DataVencimento <= lancamento.Data)) &&
                x.StatusContaBancaria == StatusContaBancaria.EmAberto
            ).AsNoTracking().OrderByDescending(x => x.DataVencimento).ToList();

            return sugestoes != null ? 
                sugestoes.Select(
                    x => new ConciliacaoBancariaItemContaFinanceira
                    {
                        ConciliacaoBancariaItemId = lancamento.Id,
                        ContaFinanceiraId = x.Id,
                        ValorConciliado = x.ValorPrevisto,
                        ContaFinanceira = x//navigation para exibir as informações em tela
                    }).ToList() : null;
        }

        public List<ConciliacaoBancariaItem> GetConciliacaoBancariaItemSugestoes(Guid conciliacaoBancariaId, int skipRecords, int pageSize)
        {
            //lista todos, mas só busca sugestões para os não conciliados total
            if (conciliacaoBancariaId == null)
                throw new BusinessException("Informe o id da conciliação bancária");
            else if(conciliacaoBancariaId == default(Guid))
                return new List<ConciliacaoBancariaItem>();
            else
            {
                //somente os que estão em aberto para conciliar
                var conciliacaoBancariaItens = conciliacaoBancariaItemBL.All.Where(x => x.ConciliacaoBancariaId == conciliacaoBancariaId && x.StatusConciliado != StatusConciliado.Sim)
                    .AsNoTracking().OrderByDescending(x => x.Data).Skip(skipRecords).Take(pageSize).ToList();

                foreach (var item in conciliacaoBancariaItens.Where(x => x.StatusConciliado == StatusConciliado.Nao))
                {
                    item.ConciliacaoBancariaItemContasFinanceiras = BuscaSugestoesPorItem(item);
                }

                return conciliacaoBancariaItens;
            }
        }

         private void BaixarContaFinanceiraSugestao(ConciliacaoBancariaItemContaFinanceira entity)
        {
            if (entity.ContaFinanceiraId != null && entity.ContaFinanceiraId != default(Guid) && entity.ContaFinanceira == null)
            {
                var conta = contaFinanceiraBL.All.Where(x => x.Id == entity.ContaFinanceiraId).FirstOrDefault();
                if (conta != null)
                {
                    var conciliacaoBancariaItem = conciliacaoBancariaItemBL.All.FirstOrDefault(x => x.Id == entity.ConciliacaoBancariaItemId);

                    if (conciliacaoBancariaItem == null)
                        entity.Fail(true, new Error("Id de lançamento da conciliação bancária inválido", "conciliacaoBancariaItemId"));
                    else
                    {
                        SalvaContaFinanceiraBaixa(entity, conciliacaoBancariaItem);
                    }
                }
            }
        }
        
        public override void Insert(ConciliacaoBancariaItemContaFinanceira entity)
        {
            var conciliacaoBancariaItem = conciliacaoBancariaItemBL.All.FirstOrDefault(x => x.Id == entity.ConciliacaoBancariaItemId);
            if (conciliacaoBancariaItem == null)
                throw new BusinessException("Id de lançamento da conciliação bancária inválido");

            //Já existe a contaFinanceiraId informada ou vai associar a conta que esta na navigation
            //mesma transacao, ainda não estava salvo para usar do contafinanceirabl.all.where 
            ValidaModel(entity, conciliacaoBancariaItem);
            BaixarContaFinanceiraSugestao(entity);
            entity.ContaFinanceira = null;

            base.Insert(entity);
        }

        public void SalvarConciliacaoBuscarExistentes(ConciliacaoBancariaItem entity)
        {
            entity.UsuarioInclusao = AppUser;
            entity.DataInclusao = DateTime.Now;
            entity.EmpresaId = EmpresaId;

            var conciliacaoBancariaItem = conciliacaoBancariaItemBL.All.FirstOrDefault(x => x.Id == entity.Id);

            if (conciliacaoBancariaItem == null)
                throw new BusinessException("Id de lançamento da conciliação bancária inválido");
            else if (entity.ConciliacaoBancariaItemContasFinanceiras == null || !entity.ConciliacaoBancariaItemContasFinanceiras.Any())
                throw new BusinessException("Necessário informar as contas financeiras a serem conciliadas");
            else
            {
                double somaConciliados = Math.Round(entity.ConciliacaoBancariaItemContasFinanceiras.Sum(x => x.ValorConciliado), 2);
                double valor = Math.Round(Math.Abs(conciliacaoBancariaItem.Valor), 2);
                if (somaConciliados > valor)
                    throw new BusinessException("A soma dos valores conciliados não pode ser superior ao valor do lançamento");
                else if (somaConciliados <= 0)
                    throw new BusinessException("A soma dos valores conciliados deve ser superior a zero");
                else if (!(somaConciliados == Math.Round(Math.Abs(conciliacaoBancariaItem.Valor),2)))
                    throw new BusinessException("A soma dos valores conciliados deve ser igual ao valor do lançamento no extrato");
            }

            double somaValorConciliados = 0;//pois não estão no all. para validar
            foreach (var item in entity.ConciliacaoBancariaItemContasFinanceiras)
            {
                ValidaModel(item, conciliacaoBancariaItem, somaValorConciliados);
                somaValorConciliados += item.ValorConciliado;

                SalvaContaFinanceiraBaixa(item, conciliacaoBancariaItem);
                base.Insert(item);
            }
            entity.ConciliacaoBancariaItemContasFinanceiras = null;
        }

        public override void Update(ConciliacaoBancariaItemContaFinanceira entity)
        {
            throw new BusinessException("Não é possível atualizar a relação da conta financeira conciliada e sua respectiva baixa");
        }

        public override void Delete(ConciliacaoBancariaItemContaFinanceira entityToDelete)
        {
            throw new BusinessException("Não é possível deletar a relação da conta financeira conciliada e sua respectiva baixa");
        }
    }
}