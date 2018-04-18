using Fly01.Estoque.Domain.Entities;
using Fly01.Core.BL;
using System.Linq;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Estoque.BL
{
    public class PosicaoAtualBL : PlataformaBaseBL<PosicaoAtual>
    {
        protected ProdutoBL ProdutoBL;

        public PosicaoAtualBL(AppDataContextBase context, ProdutoBL produtoBL)
            : base(context)
        {
            ProdutoBL = produtoBL;
        }

        public PosicaoAtual Get()
        {
            var posicaoAtual = new PosicaoAtual(ProdutoBL.AllIncluding(x => x.UnidadeMedida).ToList())
            {
                PlataformaId = PlataformaUrl, // TODO: Ver se tem como não precisar passar PlataformaId
                UsuarioInclusao = AppUser // TODO: Ver se tem como não precisar passar UsuarioInclusao
            };

            return posicaoAtual;
        }
    }
}