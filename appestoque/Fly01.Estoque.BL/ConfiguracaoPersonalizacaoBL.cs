using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class ConfiguracaoPersonalizacaoBL : PlataformaBaseBL<ConfiguracaoPersonalizacao>
    {
        public ConfiguracaoPersonalizacaoBL(AppDataContextBase context) : base(context)
        {
            MustConsumeMessageServiceBus = true;
        }
    }
}