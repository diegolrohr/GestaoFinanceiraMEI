using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
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

        public override void Update(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            var canUpdate = previous.Status == StatusOrdemServico.EmAberto || previous.Status == StatusOrdemServico.EmAndamento || previous.Status == StatusOrdemServico.EmPreenchimento;
            entity.Fail(!canUpdate, new Error("Somente ordens em aberto e em andamento podem ser alteradas", "status"));
            if (canUpdate)
            {
                entity.Fail(previous.Status == StatusOrdemServico.EmAndamento && entity.Status == StatusOrdemServico.EmAberto, new Error("Não é possível alterar o status de 'Em Andamento' para 'Em Aberto'", "status"));
                entity.Fail(previous.Status == StatusOrdemServico.EmAndamento && entity.Status == StatusOrdemServico.EmAberto, new Error("Não é possível alterar o status de 'Em Andamento' para 'Em Aberto'", "status"));
                entity.Fail(previous.Status != StatusOrdemServico.EmAberto && entity.Status == StatusOrdemServico.EmAndamento, new Error("Somente ordens 'Em Aberto' podem se tornar 'Em Andamento'", "status"));
                if (previous.Status == StatusOrdemServico.EmPreenchimento || previous.Status == StatusOrdemServico.EmAberto)
                    entity.Fail(!OrdemServicoItemServicoBL.All.AsNoTracking().Any(x => x.OrdemServicoId == entity.Id), new Error("É preciso informar ao menos um serviço", "status"));
                entity.Fail(previous.Numero != entity.Numero, new Error("Não é permitido alterar o número da OS", "status"));
            }

            base.Update(entity);
        }

        public override void Delete(Core.Entities.Domains.Commons.OrdemServico entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemServico.EmAberto, new Error("Somente ordens em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }

        public TotalOrdemServico CalculaTotalOrdemServico(Guid ordemServicoId, bool onList = false)
        {
            var ordemServico = All.Where(x => x.Id == ordemServicoId).FirstOrDefault();

            var produtos = OrdemServicoItemProdutoBL.All.Where(x => x.OrdemServicoId == ordemServicoId).ToList();
            var totalProdutos = produtos != null ? produtos.Sum(x => ((x.Quantidade * x.Valor) - x.Desconto)) : 0.0;
            var servicos = OrdemServicoItemServicoBL.All.AsNoTracking().Where(x => x.OrdemServicoId == ordemServicoId).ToList();
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
    }
}
