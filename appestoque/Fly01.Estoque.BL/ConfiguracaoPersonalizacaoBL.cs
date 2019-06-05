using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Notifications;
using System.Data.Entity;
using System.Linq;

namespace Fly01.Estoque.BL
{
    public class ConfiguracaoPersonalizacaoBL : PlataformaBaseBL<ConfiguracaoPersonalizacao>
    {
        public ConfiguracaoPersonalizacaoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }

        public override void ValidaModel(ConfiguracaoPersonalizacao entity)
        {
            entity.Fail(All.AsNoTracking().Any(x => x.Id != entity.Id), new Error("Já existe um registro de configuração de personalização, atualize o existente, não é possível ter mais de 1 registro.", "id"));
            entity.Fail(!entity.ExibirStepProdutosVendas && !entity.ExibirStepServicosVendas, new Error("Não é possível desmarcar Produtos e Serviços simultaneamente. É necessário que pelo menos uma das opções esteja habilitada.", ""));
            base.ValidaModel(entity);
        }
    }
}