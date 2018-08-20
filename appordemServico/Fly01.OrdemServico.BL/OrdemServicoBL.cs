using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Enum;
using Fly01.Core.Notifications;
using System.Data.Entity;
using System.Linq;

namespace Fly01.OrdemServico.BL
{
    public class OrdemServicoBL : PlataformaBaseBL<Core.Entities.Domains.Commons.OrdemServico>
    {
        public const int MaxLengthObservacao = 200;

        protected OrdemServicoItemServicoBL OrdemServicoItemServicoBL { get; set; }
        protected OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL { get; set; }
        protected OrdemServicoManutencaoBL OrdemServicoManutencaoBL { get; set; }

        public OrdemServicoBL(AppDataContextBase context,
                              OrdemServicoItemServicoBL ordemServicoItemServicoBL,
                              OrdemServicoItemProdutoBL ordemServicoItemProdutoBL,
                              OrdemServicoManutencaoBL ordemServicoManutencaoBL) : base(context)
        {
            OrdemServicoItemServicoBL = ordemServicoItemServicoBL;
            OrdemServicoItemProdutoBL = ordemServicoItemProdutoBL;
            OrdemServicoManutencaoBL = ordemServicoManutencaoBL;
        }

        public IQueryable<Core.Entities.Domains.Commons.OrdemServico> Everything => repository.All.Where(x => x.PlataformaId == PlataformaUrl);

        public override void ValidaModel(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            entity.Fail(entity.Observacao != null && entity.Observacao.Length > MaxLengthObservacao, new Error($"A observacao não poder ter mais de {MaxLengthObservacao} caracteres", "observacao"));
            entity.Fail(entity.Numero < 1, new Error("O número do orçamento/pedido é inválido"));

            base.ValidaModel(entity);
        }

        public override void Insert(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            base.Insert(entity);

            //Só obtém próximo número depois que tudo foi validado, para poupar sequenciais no caso defalha
            entity.Numero = ObterProximoNumero();
        }

        private int ObterProximoNumero() => Everything.Max(x => x.Numero) + 1;

        public override void Update(Core.Entities.Domains.Commons.OrdemServico entity)
        {
            base.Update(entity);
            var previous = All.AsNoTracking().FirstOrDefault(e => e.Id == entity.Id);
            entity.Fail(previous.Status != StatusOrdemServico.EmAberto && previous.Status != StatusOrdemServico.EmAndamento, new Error("Somente ordens em aberto e em andamento podem ser alteradas", "status"));
            entity.Fail(previous.Status == StatusOrdemServico.EmAndamento && entity.Status != StatusOrdemServico.EmAberto, new Error("Não é possível alterar o status de 'Em Andamento' para 'Em Aberto'", "status"));
            entity.Fail(previous.Status == StatusOrdemServico.EmAndamento && entity.Status != StatusOrdemServico.EmAberto, new Error("Não é possível alterar o status de 'Em Andamento' para 'Em Aberto'", "status"));

            base.Update(entity);
        }

        public override void Delete(Core.Entities.Domains.Commons.OrdemServico entityToDelete)
        {
            entityToDelete.Fail(entityToDelete.Status != StatusOrdemServico.EmAberto, new Error("Somente ordens em aberto pode ser deletada", "status"));

            if (!entityToDelete.IsValid())
                throw new BusinessException(entityToDelete.Notification.Get());

            base.Delete(entityToDelete);
        }
    }
}
