using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
using Fly01.OrdemServico.DAL;
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

        private CidadeBL cidadeBL;
        public CidadeBL CidadeBL => cidadeBL ?? (cidadeBL = new CidadeBL(Context));

        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private GrupoProdutoBL grupoProdutoBL;
        public GrupoProdutoBL GrupoProdutoBL => grupoProdutoBL ?? (grupoProdutoBL = new GrupoProdutoBL(Context));

        private OrdemServicoBL ordemServicoBL;
        public OrdemServicoBL OrdemServicoBL => ordemServicoBL ?? (ordemServicoBL = new OrdemServicoBL(Context, ParametroOrdemServicoBL, PessoaBL, OrdemServicoItemProdutoBL, OrdemServicoItemServicoBL, OrdemServicoManutencaoBL, KitItemBL));

        private OrdemServicoItemProdutoBL ordemServicoItemProdutoBL;
        public OrdemServicoItemProdutoBL OrdemServicoItemProdutoBL => ordemServicoItemProdutoBL ?? (ordemServicoItemProdutoBL = new OrdemServicoItemProdutoBL(Context, ProdutoBL));

        private OrdemServicoItemServicoBL ordemServicoItemServicoBL;
        public OrdemServicoItemServicoBL OrdemServicoItemServicoBL => ordemServicoItemServicoBL ?? (ordemServicoItemServicoBL = new OrdemServicoItemServicoBL(Context, ServicoBL));

        private OrdemServicoManutencaoBL ordemServicoManutencaoBL;
        public OrdemServicoManutencaoBL OrdemServicoManutencaoBL => ordemServicoManutencaoBL ?? (ordemServicoManutencaoBL = new OrdemServicoManutencaoBL(Context, ProdutoBL));

        private PessoaBL pessoaBL;
        public PessoaBL PessoaBL => pessoaBL ?? (pessoaBL = new PessoaBL(Context, EstadoBL, CidadeBL));

        private ProdutoBL produtoBL;
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL, NCMBL, UnidadeMedidaBL, CestBL, EnquadramentoLegalIPIBL));

        private CestBL cestBL;
        public CestBL CestBL => cestBL ?? (cestBL = new CestBL(Context));

        private ServicoBL servicoBL;
        public ServicoBL ServicoBL => servicoBL ?? (servicoBL = new ServicoBL(Context, ISSBL, NBSBL, UnidadeMedidaBL));

        private UnidadeMedidaBL unidadeMedidaBL;
        public UnidadeMedidaBL UnidadeMedidaBL => unidadeMedidaBL ?? (unidadeMedidaBL = new UnidadeMedidaBL(Context));

        private NCMBL ncmBL;
        public NCMBL NCMBL => ncmBL ?? (ncmBL = new NCMBL(Context));

        private NBSBL nbsBL;
        public NBSBL NBSBL => nbsBL ?? (nbsBL = new NBSBL(Context));

        private ParametroOrdemServicoBL parametroOrdemServicoBL;
        public ParametroOrdemServicoBL ParametroOrdemServicoBL => parametroOrdemServicoBL ?? (parametroOrdemServicoBL = new ParametroOrdemServicoBL(Context));

        private DashboardBL dashboardBL;
        public DashboardBL DashboardBL => dashboardBL ?? (dashboardBL = new DashboardBL(Context, OrdemServicoBL, OrdemServicoItemServicoBL, OrdemServicoItemProdutoBL));

        private EnquadramentoLegalIPIBL enquadramentoLegalIPIBL;
        public EnquadramentoLegalIPIBL EnquadramentoLegalIPIBL => enquadramentoLegalIPIBL ?? (enquadramentoLegalIPIBL = new EnquadramentoLegalIPIBL(Context));

        private ISSBL issBL;
        public ISSBL ISSBL => issBL ?? (issBL = new ISSBL(Context));

        private ArquivoBL arquivoBL;
        public ArquivoBL ArquivoBL => arquivoBL ?? (arquivoBL = new ArquivoBL(Context, PessoaBL, ProdutoBL, GrupoProdutoBL, UnidadeMedidaBL));

        private KitBL kitBL;
        public KitBL KitBL => kitBL ?? (kitBL = new KitBL(Context));

        private KitItemBL kitItemBL;
        public KitItemBL KitItemBL => kitItemBL ?? (kitItemBL = new KitItemBL(Context, KitBL, ProdutoBL, ServicoBL));

        private AgendaBL agendaBL;
        public AgendaBL AgendaBL => agendaBL ?? (agendaBL = new AgendaBL(Context, OrdemServicoBL));

        private PaisBL paisBL;
        public PaisBL PaisBL => paisBL ?? (paisBL = new PaisBL(Context));

        #endregion
    }
}