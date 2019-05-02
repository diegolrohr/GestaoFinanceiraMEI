using Fly01.Compras.DAL;
using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
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
        public ArquivoBL ArquivoBL => arquivoBL ?? (arquivoBL = new ArquivoBL(Context, PessoaBL , ProdutoBL, GrupoProdutoBL, UnidadeMedidaBL));

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
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL, NCMBL, UnidadeMedidaBL, CestBL, EnquadramentoLegalIPIBL));

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
        public PedidoBL PedidoBL => pedidoBL ?? (pedidoBL = new PedidoBL(Context, PedidoItemBL, OrdemCompraBL, NFeEntradaBL, NFeProdutoEntradaBL, NotaFiscalItemTributacaoEntradaBL, TotalTributacaoBL, KitItemBL, EstadoBL));

        private PedidoItemBL pedidoItemBL;
        public PedidoItemBL PedidoItemBL => pedidoItemBL ?? (pedidoItemBL = new PedidoItemBL(Context));

        private OrcamentoBL orcamentoBL;
        public OrcamentoBL OrcamentoBL => orcamentoBL ?? (orcamentoBL = new OrcamentoBL(Context, PedidoBL, OrcamentoItemBL, PedidoItemBL, OrdemCompraBL, KitItemBL, EstadoBL));

        private OrcamentoItemBL orcamentoItemBL;
        public OrcamentoItemBL OrcamentoItemBL => orcamentoItemBL ?? (orcamentoItemBL = new OrcamentoItemBL(Context));

        private SubstituicaoTributariaBL substituicaoTributariaBL;
        public SubstituicaoTributariaBL SubstituicaoTributariaBL => substituicaoTributariaBL ?? (substituicaoTributariaBL = new SubstituicaoTributariaBL(Context, NCMBL, CestBL, EstadoBL));

        private CfopBL cfopBL;
        public CfopBL CfopBL => cfopBL ?? (cfopBL = new CfopBL(Context));

        private GrupoTributarioBL grupoTributarioBL;
        public GrupoTributarioBL GrupoTributarioBL => grupoTributarioBL ?? (grupoTributarioBL = new GrupoTributarioBL(Context, CfopBL));

        private CestBL cestBL;
        public CestBL CestBL => cestBL ?? (cestBL = new CestBL(Context));

        private DashboardBL dashboardBL;
        public DashboardBL DashboardBL => dashboardBL ?? (dashboardBL = new DashboardBL(Context, PedidoBL, PedidoItemBL, OrdemCompraBL));

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

        private NotaFiscalEntradaBL notaFiscalEntradaBL;
        public NotaFiscalEntradaBL NotaFiscalEntradaBL => notaFiscalEntradaBL ?? (notaFiscalEntradaBL = new NotaFiscalEntradaBL(Context, NFeEntradaBL, CertificadoDigitalBL, TotalTributacaoBL, SerieNotaFiscalBL, NotaFiscalInutilizadaBL));

        private NFeEntradaBL nfeEntradaBL;
        public NFeEntradaBL NFeEntradaBL => nfeEntradaBL ?? (nfeEntradaBL = new NFeEntradaBL(Context, SerieNotaFiscalBL, NFeProdutoEntradaBL, TotalTributacaoBL, CertificadoDigitalBL, PessoaBL, CondicaoParcelamentoBL, SubstituicaoTributariaBL, NotaFiscalItemTributacaoEntradaBL, FormaPagamentoBL, NotaFiscalInutilizadaBL, EstadoBL));

        private NFeProdutoEntradaBL nfeProdutoEntradaBL;
        public NFeProdutoEntradaBL NFeProdutoEntradaBL => nfeProdutoEntradaBL ?? (nfeProdutoEntradaBL = new NFeProdutoEntradaBL(Context));

        private SerieNotaFiscalBL serieNotaFiscalBL;
        public SerieNotaFiscalBL SerieNotaFiscalBL => serieNotaFiscalBL ?? (serieNotaFiscalBL = new SerieNotaFiscalBL(Context, NotaFiscalInutilizadaBL));

        private TotalTributacaoBL totalTributacaoBL;
        public TotalTributacaoBL TotalTributacaoBL => totalTributacaoBL ?? (totalTributacaoBL = new TotalTributacaoBL(Context, PessoaBL, GrupoTributarioBL, ProdutoBL, SubstituicaoTributariaBL, ParametroTributarioBL, CertificadoDigitalBL, PedidoItemBL));

        private NotaFiscalItemTributacaoEntradaBL notaFiscalItemTributacaoEntradaBL;
        public NotaFiscalItemTributacaoEntradaBL NotaFiscalItemTributacaoEntradaBL => notaFiscalItemTributacaoEntradaBL ?? (notaFiscalItemTributacaoEntradaBL = new NotaFiscalItemTributacaoEntradaBL(Context));

        private NotaFiscalCartaCorrecaoEntradaBL notaFiscalCartaCorrecaoEntradaBL;
        public NotaFiscalCartaCorrecaoEntradaBL NotaFiscalCartaCorrecaoEntradaBL => notaFiscalCartaCorrecaoEntradaBL ?? (notaFiscalCartaCorrecaoEntradaBL = new NotaFiscalCartaCorrecaoEntradaBL(Context, NotaFiscalEntradaBL, TotalTributacaoBL, CertificadoDigitalBL));

        private MonitorNFBL monitorNFBL;
        public MonitorNFBL MonitorNFBL => monitorNFBL ?? (monitorNFBL = new MonitorNFBL(Context, TotalTributacaoBL, NFeEntradaBL, CertificadoDigitalBL, NotaFiscalInutilizadaBL, NotaFiscalCartaCorrecaoEntradaBL));

        private ServicoBL servicoBL;
        public ServicoBL ServicoBL => servicoBL ?? (servicoBL = new ServicoBL(Context, ISSBL, NBSBL, UnidadeMedidaBL));

        private ISSBL issBL;
        public ISSBL ISSBL => issBL ?? (issBL = new ISSBL(Context));

        private NBSBL nbsBL;
        public NBSBL NBSBL => nbsBL ?? (nbsBL = new NBSBL(Context));

        private KitBL kitBL;
        public KitBL KitBL => kitBL ?? (kitBL = new KitBL(Context));

        private KitItemBL kitItemBL;
        public KitItemBL KitItemBL => kitItemBL ?? (kitItemBL = new KitItemBL(Context, KitBL, ProdutoBL, ServicoBL));

        private NFeImportacaoBL nfeImportacaoBL;
        public NFeImportacaoBL NFeImportacaoBL => nfeImportacaoBL ?? (nfeImportacaoBL = new NFeImportacaoBL(Context, NFeImportacaoProdutoBL, PessoaBL, ProdutoBL, PedidoBL, PedidoItemBL, UnidadeMedidaBL, NFeImportacaoCobrancaBL));

        private NFeImportacaoProdutoBL nfeImportacaoProdutoBL;
        public NFeImportacaoProdutoBL NFeImportacaoProdutoBL => nfeImportacaoProdutoBL ?? (nfeImportacaoProdutoBL = new NFeImportacaoProdutoBL(Context, ProdutoBL));

        private NFeImportacaoCobrancaBL nfeImportacaoCobrancaBL;
        public NFeImportacaoCobrancaBL NFeImportacaoCobrancaBL => nfeImportacaoCobrancaBL ?? (nfeImportacaoCobrancaBL = new NFeImportacaoCobrancaBL(Context));

        private CentroCustoBL centroCustoBL;
        public CentroCustoBL CentroCustoBL => centroCustoBL ?? (centroCustoBL = new CentroCustoBL(Context));
        #endregion
    }
}
