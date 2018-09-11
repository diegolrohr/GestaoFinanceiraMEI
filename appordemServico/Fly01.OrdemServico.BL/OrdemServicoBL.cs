using Fly01.Core.BL;
using Fly01.Core.Entities.Domains;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using Fly01.Core.ServiceBus;
using Fly01.OrdemServico.BL.Extension;
using System;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoBL : PlataformaBaseBL<Core.Entities.Domains.Commons.OrdemServico>
    {
        public const int MaxLengthObservacao = 200;
        private readonly ParametroOrdemServicoBL _parametroBL;
        private readonly PessoaBL _pessoaBL;
        protected OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL { get; set; }
        protected OrdemServicoItemServicoBL OrdemServicoItemServicoBL { get; set; }
        protected OrdemServicoManutencaoBL OrdemServicoManutencaoBL { get; set; }

        public OrdemServicoBL(AppDataContextBase context, ParametroOrdemServicoBL parametroBL, PessoaBL pessoaBL, OrdemServicoItemProdutoBL ordemServicoItemProdutoBL, OrdemServicoItemServicoBL ordemServicoItemServicoBL, OrdemServicoManutencaoBL ordemServicoManutencaoBL) : base(context)
        {
            OrdemServicoManutencaoBL = ordemServicoManutencaoBL;
            OrdemServicoItemProdutoBL = ordemServicoItemProdutoBL;
            OrdemServicoItemServicoBL = ordemServicoItemServicoBL;
            _parametroBL = parametroBL;
            _pessoaBL = pessoaBL;
        }

        public IQueryable<Core.Entities.Domains.Commons.OrdemServico> Everything => repository.All.AsNoTracking().Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            entity.Fail(entity.Descricao != null && entity.Descricao.Length > MaxLengthObservacao, new Error($"A observacao não poder ter mais de {MaxLengthObservacao} caracteres", "observacao"));
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

            base.Insert(entity);

            //Só obtém próximo número depois que tudo foi validado, para poupar sequenciais no caso defalha
            entity.Numero = ObterProximoNumero();
        }

        private int ObterProximoNumero() => (Everything.Max(x => (int?)x.Numero) ?? 0) + 1;

        public void GerarOrdemVenda(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            var concluida = previous.Status == StatusOrdemServico.Concluido;
            previous.Fail(!concluida, new Error("Apenas ordens de serviço concluídas podem gerar ordem de venda!"));
            if (concluida)
                previous.Fail(previous.GeraOrdemVenda, new Error("Ordem de Venda já foi gerada"));

            base.Update(entity);
        }

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
                TipoVenda = TipoVenda.Normal,
                Status = StatusOrdemVenda.Aberto,
                TipoFrete = TipoFrete.SemFrete,
                TipoNfeComplementar = TipoNfeComplementar.NaoComplementar,
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
    }
}
