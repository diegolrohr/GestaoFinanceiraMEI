using Fly01.Core.BL;
using Fly01.Compras.DAL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.BL
{
    public class NFeImportacaoProdutoBL : PlataformaBaseBL<NFeImportacaoProduto>
    {
        public NFeImportacaoProdutoBL(AppDataContext context) : base(context)
        {
        }
    }
}