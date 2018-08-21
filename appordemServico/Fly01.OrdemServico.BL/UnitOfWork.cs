using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
using Fly01.OrdemServico.API.Models.DAL;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Fly01.OrdemServico.BL
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
        private PessoaBL pessoaBL;
        public PessoaBL PessoaBL => pessoaBL ?? (pessoaBL = new PessoaBL(Context, EstadoBL, CidadeBL));

        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private CidadeBL cidadeBL;
        public CidadeBL CidadeBL => cidadeBL ?? (cidadeBL = new CidadeBL(Context));

        private NCMBL ncmBL;
        public NCMBL NCMBL => ncmBL ?? (ncmBL = new NCMBL(Context));

        private ProdutoBL produtoBL;
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL));

        private GrupoProdutoBL grupoProdutoBL;
        public GrupoProdutoBL GrupoProdutoBL => grupoProdutoBL ?? (grupoProdutoBL = new GrupoProdutoBL(Context));

        private UnidadeMedidaBL unidadeMedidaBL;
        public UnidadeMedidaBL UnidadeMedidaBL => unidadeMedidaBL ?? (unidadeMedidaBL = new UnidadeMedidaBL(Context));

        private ParametroOrdemServicoBL parametroOrdemServicoBL;
        public ParametroOrdemServicoBL ParametroOrdemServicoBL => parametroOrdemServicoBL ?? (parametroOrdemServicoBL = new ParametroOrdemServicoBL(Context));

        private OrdemServicoItemServicoBL ordemServicoItemServicoBL;
        public OrdemServicoItemServicoBL OrdemServicoItemServicoBL => ordemServicoItemServicoBL ?? (ordemServicoItemServicoBL = new OrdemServicoItemServicoBL());

        private OrdemServicoItemProdutoBL ordemServicoItemProdutoBL;
        public OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL => ordemServicoItemProdutoBL ?? (ordemServicoItemProdutoBL = new OrdemServicoItemProdutoBL());

        private OrdemServicoManutencaoBL ordemServicoManutencaoBL;
        public OrdemServicoManutencaoBL OrdemServicoManutencaoBL => ordemServicoManutencaoBL ?? (ordemServicoManutencaoBL = new OrdemServicoManutencaoBL());

        private OrdemServicoBL ordemServicoBL;
        public OrdemServicoBL OrdemServicoBL => ordemServicoBL ?? (ordemServicoBL = new OrdemServicoBL(Context, OrdemServicoItemServicoBL, OrdemServicoItemProdutoBL, OrdemServicoManutencaoBL));

        private DashboardBL dashboardBL;
        public DashboardBL DashboardBL => dashboardBL ?? (dashboardBL = new DashboardBL(Context, OrdemServicoBL, ProdutoBL));

        #endregion
    }
}