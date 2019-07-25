using Fly01.Core.BL;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.Core.ViewModels.Presentation.Commons;
using Fly01.OrdemServico.BL.Extension;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoBL : PlataformaBaseBL<Core.Entities.Domains.Commons.OrdemServico>
    {
        public const int MaxLengthObservacao = 1000;
        private readonly ParametroOrdemServicoBL _parametroBL;
        private readonly PessoaBL _pessoaBL;
        protected OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL { get; set; }
        protected OrdemServicoItemServicoBL OrdemServicoItemServicoBL { get; set; }
        protected OrdemServicoManutencaoBL OrdemServicoManutencaoBL { get; set; }
        protected KitItemBL KitItemBL { get; set; }

        public OrdemServicoBL(AppDataContextBase context, ParametroOrdemServicoBL parametroBL, PessoaBL pessoaBL, OrdemServicoItemProdutoBL ordemServicoItemProdutoBL, OrdemServicoItemServicoBL ordemServicoItemServicoBL, OrdemServicoManutencaoBL ordemServicoManutencaoBL, KitItemBL kitItemBl) : base(context)
        {
            OrdemServicoManutencaoBL = ordemServicoManutencaoBL;
            OrdemServicoItemProdutoBL = ordemServicoItemProdutoBL;
            OrdemServicoItemServicoBL = ordemServicoItemServicoBL;
            _parametroBL = parametroBL;
            _pessoaBL = pessoaBL;
            KitItemBL = kitItemBl;
        }

        public IQueryable<Core.Entities.Domains.Commons.OrdemServico> Everything => repository.All.AsNoTracking().Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            entity.Fail(entity.Descricao != null && entity.Descricao.Length > MaxLengthObservacao, new Error($"A observação não poder ter mais de {MaxLengthObservacao} caracteres", "observacao"));
            if (entity.DataEntrega == DateTime.MinValue)
                entity.DataEntrega = entity.DataEmissao.AddDays(_parametroBL.ParametroPlataforma.DiasPrazoEntrega);
            else
                entity.Fail(entity.DataEntrega < entity.DataEntrega, new Error("A Data de entrega deve ser maior ou igual à de emissão!", "dataEntrega"));

            var responsavel = entity.ValidForeignKey(x => x.ResponsavelId, "Responsável", "responsavelId", _pessoaBL, x => new
            {
                x.Id,
                x.Vendedor
            });

            if (responsavel != null)
                entity.Fail(!responsavel.Vendedor, new Error("A pessoa escolhida como responsável deve estar marcada como vendedor em seu cadastro!", "responsavelId"));

            base.ValidaModel(entity);
        }

        public override void Insert(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            entity.Status = StatusOrdemServico.EmPreenchimento;

            entity.Tempo = entity.Duracao.TotalMinutes;

            base.Insert(entity);

            //Só obtém próximo número depois que tudo foi validado, para poupar sequenciais no caso defalha
            entity.Numero = ObterProximoNumero();
        }

        private int ObterProximoNumero() => (Everything.Max(x => (int?)x.Numero) ?? 0) + 1;

        public override void Update(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            var canUpdate = previous.Status == StatusOrdemServico.EmAberto || previous.Status == StatusOrdemServico.EmAndamento || previous.Status == StatusOrdemServico.EmPreenchimento;
            entity.Fail(!canUpdate, new Error("Somente ordens em aberto e em andamento podem ser alteradas", "status"));
            if (canUpdate)
            {
                if (previous.Status != entity.Status)
                {
                    entity.Fail(previous.Status != StatusOrdemServico.EmPreenchimento && entity.Status == StatusOrdemServico.EmAberto, new Error("Apenas ordens 'Em Preenchimento' podem se tornar 'Em Aberto'", "status"));
                    entity.Fail(previous.Status != StatusOrdemServico.EmAberto && entity.Status == StatusOrdemServico.EmAndamento, new Error("Somente ordens 'Em Aberto' podem se tornar 'Em Andamento'", "status"));
                    entity.Fail(previous.Status != StatusOrdemServico.EmAndamento && entity.Status == StatusOrdemServico.Concluido, new Error("Somente ordens 'Em Andamento' podem se tornar 'Concluído'", "status"));
                    entity.Fail(previous.Status == StatusOrdemServico.EmPreenchimento && entity.Status == StatusOrdemServico.Cancelado, new Error("Ordens 'Em Preenchimento' não podem ser canceladas. Conclua o preenchimento ou a exclua", "status"));
                }
                if (previous.Status == StatusOrdemServico.EmPreenchimento || previous.Status == StatusOrdemServico.EmAberto)
                    entity.Fail(!OrdemServicoItemServicoBL.All.AsNoTracking().Any(x => x.OrdemServicoId == entity.Id), new Error("É preciso informar ao menos um serviço", "status"));
                entity.Fail(previous.Numero != entity.Numero, new Error("Não é permitido alterar o número da OS", "status"));
            }

            base.Update(entity);
        }

        public override void Delete(Core.Entities.Domains.Commons.OrdemServico entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemServico.EmAberto && entityToDelete.Status != StatusOrdemServico.EmPreenchimento,
                                new Error("Somente ordens 'Em Aberto' ou 'Em Preenchimento' podem ser deletadas", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public TotalOrdemServico CalculaTotalOrdemServico(Guid ordemServicoId, bool onList = false)
        {
            var ordemServico = All.Where(x => x.Id == ordemServicoId).FirstOrDefault();

            var produtos = GetProdutos(ordemServicoId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var servicos = GetServicos(ordemServicoId).ToList();
            var totalServicos = servicos != null ? servicos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var itensCliente = OrdemServicoManutencaoBL.All.AsNoTracking().Where(x => x.OrdemServicoId == ordemServicoId).ToList();
            var qtdItensCliente = itensCliente != null ? itensCliente.Sum(x => ((x.Quantidade))) : 0;
            var result = new TotalOrdemServico()
            {
                QuantidadeItensCliente = qtdItensCliente,
                TotalProdutos = Math.Round(totalProdutos, 2, MidpointRounding.AwayFromZero),
                TotalServicos = Math.Round(totalServicos, 2, MidpointRounding.AwayFromZero)
            };

            return result;
        }

        private IQueryable<OrdemServicoItemServico> GetServicos(Guid ordemServicoId)
            => OrdemServicoItemServicoBL.All.AsNoTracking().Where(x => x.OrdemServicoId == ordemServicoId);

        private IQueryable<OrdemServicoItemProduto> GetProdutos(Guid ordemServicoId)
            => OrdemServicoItemProdutoBL.All.Where(x => x.OrdemServicoId == ordemServicoId);

        public override void AfterSave(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            if (entity.Status != StatusOrdemServico.Concluido || !entity.GeraOrdemVenda)
                return;

            Send(new OrdemVenda
            {
                Id = entity.Id,
                Ativo = true,
                ClienteId = entity.ClienteId,
                Data = entity.DataEntrega,
                TipoOrdemVenda = TipoOrdemVenda.Pedido,
                TipoVenda = TipoCompraVenda.Normal,
                Status = Status.Aberto,
                TipoFrete = TipoFrete.SemFrete,
                TipoNfeComplementar = TipoNfeComplementar.NaoComplementar,
                Observacao = $"Pedido gerado a partir da Ordem de Serviço número {entity.Numero}",
                GeraFinanceiro = true,
                GeraNotaFiscal = true,
                PlataformaId = entity.PlataformaId
            });

            foreach (var prodOS in GetProdutos(entity.Id))
            {
                Send(new OrdemVendaProduto
                {
                    Ativo = true,
                    OrdemVendaId = entity.Id,
                    PlataformaId = prodOS.PlataformaId,
                    ProdutoId = prodOS.ProdutoId,
                    Quantidade = prodOS.Quantidade,
                    Valor = prodOS.Valor,
                    Desconto = prodOS.Desconto,
                    Total = prodOS.Total
                });
            }

            foreach (var servOS in GetServicos(entity.Id))
            {
                Send(new OrdemVendaServico
                {
                    Ativo = true,
                    OrdemVendaId = entity.Id,
                    PlataformaId = servOS.PlataformaId,
                    ServicoId = servOS.ServicoId,
                    Quantidade = servOS.Quantidade,
                    Valor = servOS.Valor,
                    Desconto = servOS.Desconto,
                    Total = servOS.Total
                });
            }
        }

        private void Send<T>(T entity) where T : DomainBase
            => Producer<T>.Send(entity.GetType().Name, AppUser, PlataformaUrl, entity, RabbitConfig.EnHttpVerb.POST);

        public void UtilizarKitOrdemServico(UtilizarKitVM entity)
        {
            try
            {
                if (All.Any(x => x.Id == entity.OrcamentoPedidoId))
                {
                    if (KitItemBL.All.Any(x => x.KitId == entity.KitId))
                    {
                        #region Produtos
                        if (entity.AdicionarProdutos)
                        {
                            var kitProdutos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Produto);

                            var existentesOrdemServico =
                                from ovp in OrdemServicoItemProdutoBL.AllIncluding(x => x.Produto).Where(x => x.OrdemServicoId == entity.OrcamentoPedidoId)
                                join ki in kitProdutos on ovp.ProdutoId equals ki.ProdutoId
                                select new { ProdutoId = ki.ProdutoId, OrdemServicoItemProdutoId = ovp.Id, Quantidade = ki.Quantidade };

                            var novasOrdemServicoItemProdutos =
                                from kit in kitProdutos
                                where !existentesOrdemServico.Select(x => x.ProdutoId).Contains(kit.ProdutoId)
                                select new
                                {                                     
                                    OrdemServicoId = entity.OrcamentoPedidoId,
                                    ProdutoId = kit.ProdutoId.Value,
                                    Valor = kit.Produto.ValorVenda,
                                    Quantidade = kit.Quantidade
                                };

                            foreach (var item in novasOrdemServicoItemProdutos)
                            {
                                OrdemServicoItemProdutoBL.Insert(new OrdemServicoItemProduto()
                                {
                                    ProdutoId = item.ProdutoId,
                                    OrdemServicoId = item.OrdemServicoId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesOrdemServico)
                                {
                                    var ordemServicoItemProduto = OrdemServicoItemProdutoBL.Find(item.OrdemServicoItemProdutoId);
                                    ordemServicoItemProduto.Quantidade += item.Quantidade;
                                    OrdemServicoItemProdutoBL.Update(ordemServicoItemProduto);
                                }
                            }
                        }
                        #endregion
                        #region Servicos
                        if (entity.AdicionarServicos)
                        {
                            var kitServicos = KitItemBL.All.Where(x => x.KitId == entity.KitId && x.TipoItem == TipoItem.Servico);

                            var existentesOrdemServico =
                                from ovs in OrdemServicoItemServicoBL.AllIncluding(x => x.Servico).Where(x => x.OrdemServicoId == entity.OrcamentoPedidoId)
                                join ki in kitServicos on ovs.ServicoId equals ki.ServicoId
                                select new { ServicoId = ki.ServicoId, OrdemServicoItemServicoId = ovs.Id, Quantidade = ki.Quantidade };

                            var novasOrdemServicoItemServicos =
                                from kit in kitServicos
                                where !existentesOrdemServico.Select(x => x.ServicoId).Contains(kit.ServicoId)
                                select new
                                {
                                    OrdemServicoId = entity.OrcamentoPedidoId,
                                    ServicoId = kit.ServicoId.Value,
                                    Valor = kit.Servico.ValorServico,
                                    Quantidade = kit.Quantidade
                                };

                            foreach (var item in novasOrdemServicoItemServicos)
                            {
                                OrdemServicoItemServicoBL.Insert(new OrdemServicoItemServico()
                                {
                                    ServicoId = item.ServicoId,
                                    OrdemServicoId = item.OrdemServicoId,
                                    Valor = item.Valor,
                                    Quantidade = item.Quantidade
                                });
                            }

                            if (entity.SomarExistentes)
                            {
                                foreach (var item in existentesOrdemServico)
                                {
                                    var ordemServicoItemServico = OrdemServicoItemServicoBL.Find(item.OrdemServicoItemServicoId);
                                    ordemServicoItemServico.Quantidade += item.Quantidade;
                                    OrdemServicoItemServicoBL.Update(ordemServicoItemServico);
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
