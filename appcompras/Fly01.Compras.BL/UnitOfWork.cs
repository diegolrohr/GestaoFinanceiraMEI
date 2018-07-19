using Fly01.Compras.DAL;
using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
using Fly01.Faturamento.BL;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Fly01.Compras.BL
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

        private ArquivoBL arquivoBL;
        public ArquivoBL ArquivoBL => arquivoBL ?? (arquivoBL = new ArquivoBL(Context, PessoaBL));

        private PessoaBL pessoaBL;
        public PessoaBL PessoaBL => pessoaBL ?? (pessoaBL = new PessoaBL(Context, EstadoBL, CidadeBL));

        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private CidadeBL cidadeBL;
        public CidadeBL CidadeBL => cidadeBL ?? (cidadeBL = new CidadeBL(Context));

        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        public CondicaoParcelamentoBL CondicaoParcelamentoBL => condicaoParcelamentoBL ?? (condicaoParcelamentoBL = new CondicaoParcelamentoBL(Context));

        private FormaPagamentoBL formaPagamentoBL;
        public FormaPagamentoBL FormaPagamentoBL => formaPagamentoBL ?? (formaPagamentoBL = new FormaPagamentoBL(Context));

        private ProdutoBL produtoBL;
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL));

        private NCMBL ncmBL;
        public NCMBL NCMBL => ncmBL ?? (ncmBL = new NCMBL(Context));

        private GrupoProdutoBL grupoProdutoBL;
        public GrupoProdutoBL GrupoProdutoBL => grupoProdutoBL ?? (grupoProdutoBL = new GrupoProdutoBL(Context));

        private UnidadeMedidaBL unidadeMedidaBL;
        public UnidadeMedidaBL UnidadeMedidaBL => unidadeMedidaBL ?? (unidadeMedidaBL = new UnidadeMedidaBL(Context));

        private CategoriaBL categoriaBL;
        public CategoriaBL CategoriaBL => categoriaBL ?? (categoriaBL = new CategoriaBL(Context, OrdemCompraBL));

        private OrdemCompraBL ordemCompraBL;
        public OrdemCompraBL OrdemCompraBL => ordemCompraBL ?? (ordemCompraBL = new OrdemCompraBL(Context));

        private OrdemCompraItemBL ordemCompraItemBL;
        public OrdemCompraItemBL OrdemCompraItemBL => ordemCompraItemBL ?? (ordemCompraItemBL = new OrdemCompraItemBL(Context));

        private PedidoBL pedidoBL;
        public PedidoBL PedidoBL => pedidoBL ?? (pedidoBL = new PedidoBL(Context, PedidoItemBL, OrdemCompraBL));

        private PedidoItemBL pedidoItemBL;
        public PedidoItemBL PedidoItemBL => pedidoItemBL ?? (pedidoItemBL = new PedidoItemBL(Context));

        private OrcamentoBL orcamentoBL;
        public OrcamentoBL OrcamentoBL => orcamentoBL ?? (orcamentoBL = new OrcamentoBL(Context, PedidoBL, OrcamentoItemBL, PedidoItemBL, OrdemCompraBL));

        private OrcamentoItemBL orcamentoItemBL;
        public OrcamentoItemBL OrcamentoItemBL => orcamentoItemBL ?? (orcamentoItemBL = new OrcamentoItemBL(Context));

        private SubstituicaoTributariaBL substituicaoTributariaBL;
        public SubstituicaoTributariaBL SubstituicaoTributariaBL => substituicaoTributariaBL ?? (substituicaoTributariaBL = new SubstituicaoTributariaBL(Context));

        private CfopBL cfopBL;
        public CfopBL CfopBL => cfopBL ?? (cfopBL = new CfopBL(Context));

        private GrupoTributarioBL grupoTributarioBL;
        public GrupoTributarioBL GrupoTributarioBL => grupoTributarioBL ?? (grupoTributarioBL = new GrupoTributarioBL(Context));

        private CestBL cestBL;
        public CestBL CestBL => cestBL ?? (cestBL = new CestBL(Context));

        private DashboardBL dashboardBL;
        public DashboardBL DashboardBL => dashboardBL ?? (dashboardBL = new DashboardBL(Context, FormaPagamentoBL, PedidoBL, PedidoItemBL, OrcamentoBL, OrcamentoItemBL, OrdemCompraBL, OrdemCompraItemBL, ProdutoBL));

        private EnquadramentoLegalIPIBL enquadramentoLegalIPIBL;
        public EnquadramentoLegalIPIBL EnquadramentoLegalIPIBL => enquadramentoLegalIPIBL ?? (enquadramentoLegalIPIBL = new EnquadramentoLegalIPIBL(Context));

        private ParametroTributarioBL parametroTributarioBL;
        public ParametroTributarioBL ParametroTributarioBL => parametroTributarioBL ?? (parametroTributarioBL = new ParametroTributarioBL(Context, EntidadeBL));

        private NotaFiscalInutilizadaBL notaFiscalInutilizadaBL;
        public NotaFiscalInutilizadaBL NotaFiscalInutilizadaBL => notaFiscalInutilizadaBL ?? (notaFiscalInutilizadaBL = new NotaFiscalInutilizadaBL(Context));

        private EntidadeBL entidadeBL;
        public EntidadeBL EntidadeBL => entidadeBL ?? (entidadeBL = new EntidadeBL(Context, EstadoBL));

        private CertificadoDigitalBL certificadoDigitalBL;
        public CertificadoDigitalBL CertificadoDigitalBL => certificadoDigitalBL ?? (certificadoDigitalBL = new CertificadoDigitalBL(Context, EstadoBL, ParametroTributarioBL));

        private NotaFiscalBL notaFiscalBL;
        public NotaFiscalBL NotaFiscalBL => notaFiscalBL ?? (notaFiscalBL = new NotaFiscalBL(Context, NFeBL, NFSeBL, CertificadoDigitalBL, TotalTributacaoBL, SerieNotaFiscalBL, NotaFiscalInutilizadaBL));

        private NFeBL nfeBL;
        public NFeBL NFeBL => nfeBL ?? (nfeBL = new NFeBL(Context, SerieNotaFiscalBL, NFeProdutoBL, TotalTributacaoBL, CertificadoDigitalBL, PessoaBL, CondicaoParcelamentoBL, SubstituicaoTributariaBL, NotaFiscalItemTributacaoBL, FormaPagamentoBL, NotaFiscalInutilizadaBL));

        private NFeProdutoBL nfeProdutoBL;
        public NFeProdutoBL NFeProdutoBL => nfeProdutoBL ?? (nfeProdutoBL = new NFeProdutoBL(Context));

        private NFSeBL nfseBL;
        public NFSeBL NFSeBL => nfseBL ?? (nfseBL = new NFSeBL(Context, SerieNotaFiscalBL, NFSeServicoBL, TotalTributacaoBL, NotaFiscalInutilizadaBL));

        private NFSeServicoBL nfseServicoBL;
        public NFSeServicoBL NFSeServicoBL => nfseServicoBL ?? (nfseServicoBL = new NFSeServicoBL(Context));

        private SerieNotaFiscalBL serieNotaFiscalBL;
        public SerieNotaFiscalBL SerieNotaFiscalBL => serieNotaFiscalBL ?? (serieNotaFiscalBL = new SerieNotaFiscalBL(Context, NotaFiscalInutilizadaBL));

        private TotalTributacaoBL totalTributacaoBL;
        public TotalTributacaoBL TotalTributacaoBL => totalTributacaoBL ?? (totalTributacaoBL = new TotalTributacaoBL(Context, PessoaBL, GrupoTributarioBL, ProdutoBL, SubstituicaoTributariaBL, ParametroTributarioBL, CertificadoDigitalBL));

        private NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL;
        public NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL => notaFiscalItemTributacaoBL ?? (notaFiscalItemTributacaoBL = new NotaFiscalItemTributacaoBL(Context));

        private NotaFiscalCartaCorrecaoBL notaFiscalCartaCorrecaoBL;
        public NotaFiscalCartaCorrecaoBL NotaFiscalCartaCorrecaoBL => notaFiscalCartaCorrecaoBL ?? (notaFiscalCartaCorrecaoBL = new NotaFiscalCartaCorrecaoBL(Context, NotaFiscalBL, TotalTributacaoBL, CertificadoDigitalBL));

        private MonitorNFBL monitorNFBL;
        public MonitorNFBL MonitorNFBL => monitorNFBL ?? (monitorNFBL = new MonitorNFBL(Context, TotalTributacaoBL, NFeBL, NFSeBL, NotaFiscalBL, CertificadoDigitalBL, NotaFiscalInutilizadaBL, NotaFiscalCartaCorrecaoBL));


        #endregion
    }
}
