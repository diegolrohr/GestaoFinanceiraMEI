using Fly01.Estoque.DAL;
using Fly01.Core.Base;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains;

namespace Fly01.Estoque.BL
{
    public class UnitOfWork : UnitOfWorkBase
    {
        protected override IEnumerable<DbEntityEntry> ContextChangeTrackerEntries()
        {
            return Context.ChangeTracker.Entries();
        }

        protected override async Task ContextSaveChanges()
        {
            await Context.SaveChanges();
        }

        protected override void ContextDispose()
        {
            Context.Dispose();
        }

        public AppDataContext Context;
        public UnitOfWork(ContextInitialize initialize)
        {            
            Context = new AppDataContext(initialize);
        }
        
        #region BLS

        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private TipoMovimentoBL tipoMovimentoBL;
        public TipoMovimentoBL TipoMovimentoBL => tipoMovimentoBL ?? (tipoMovimentoBL = new TipoMovimentoBL(Context));

        private NCMBL ncmBL;
        public NCMBL NCMBL => ncmBL ?? (ncmBL = new NCMBL(Context));
        
        private GrupoProdutoBL grupoProdutoBL;
        public GrupoProdutoBL GrupoProdutoBL => grupoProdutoBL ?? (grupoProdutoBL = new GrupoProdutoBL(Context));

        private UnidadeMedidaBL unidadeMedidaBL;
        public UnidadeMedidaBL UnidadeMedidaBL => unidadeMedidaBL ?? (unidadeMedidaBL = new UnidadeMedidaBL(Context));

        private InventarioItemBL inventarioItemBL;
        public InventarioItemBL InventarioItemBL => inventarioItemBL ?? (inventarioItemBL = new InventarioItemBL(Context, MovimentoBL));

        private InventarioBL inventarioBL;
        public InventarioBL InventarioBL => inventarioBL ?? (inventarioBL = new InventarioBL(Context, InventarioItemBL));

        private ProdutoBL produtoBL;
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL));

        private PosicaoAtualBL posicaoAtualBL;
        public PosicaoAtualBL PosicaoAtualBL => posicaoAtualBL ?? (posicaoAtualBL = new PosicaoAtualBL(Context, ProdutoBL));

        private AjusteManualBL ajusteManualBL;
        public AjusteManualBL AjusteManualBL => ajusteManualBL ?? (ajusteManualBL = new AjusteManualBL(Context, MovimentoBL, ProdutoBL, InventarioItemBL));

        private MovimentoBL movimentoBL;
        public MovimentoBL MovimentoBL => movimentoBL ?? (movimentoBL = new MovimentoBL(Context, ProdutoBL, TipoMovimentoBL));

        private ProdutosMaisMovimentadosBL produtosMaisMovimentadosBL;
        public ProdutosMaisMovimentadosBL ProdutosMaisMovimentadosBL => produtosMaisMovimentadosBL ?? (produtosMaisMovimentadosBL = new ProdutosMaisMovimentadosBL(Context, MovimentoBL));

        private ProdutosMenosMovimentadosBL produtosMenosMovimentadosBL;
        public ProdutosMenosMovimentadosBL ProdutosMenosMovimentadosBL => produtosMenosMovimentadosBL ?? (produtosMenosMovimentadosBL = new ProdutosMenosMovimentadosBL(Context, MovimentoBL));

        private CestBL cestBL;
        public CestBL CestBL => cestBL ?? (cestBL = new CestBL(Context));

        private MovimentoOrdemVendaBL movimentoOrdemVendaBL;
        public MovimentoOrdemVendaBL MovimentoOrdemVendaBL => movimentoOrdemVendaBL ?? (movimentoOrdemVendaBL = new MovimentoOrdemVendaBL(Context, ProdutoBL, MovimentoBL));

        private EnquadramentoLegalIPIBL enquadramentoLegalIPIBL;
        public EnquadramentoLegalIPIBL EnquadramentoLegalIPIBL => enquadramentoLegalIPIBL ?? (enquadramentoLegalIPIBL = new EnquadramentoLegalIPIBL(Context));
        #endregion
    }
}