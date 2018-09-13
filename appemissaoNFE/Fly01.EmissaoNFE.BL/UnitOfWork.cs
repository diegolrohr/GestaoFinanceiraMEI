using Fly01.EmissaoNFE.DAL;
using Fly01.Core.Base;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains;

namespace Fly01.EmissaoNFE.BL
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

        #region BLS de Impostos
        private CidadeBL cidadeBL;
        public CidadeBL CidadeBL => cidadeBL ?? (cidadeBL = new CidadeBL(Context));

        private DifalBL difalBL;
        public DifalBL DifalBL => difalBL ?? (difalBL = new DifalBL(Context));
        
        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private FcpBL fcpBL;
        public FcpBL FcpBL => fcpBL ?? (fcpBL = new FcpBL(Context));

        private FcpStBL fcpStBL;
        public FcpStBL FcpStBL => fcpStBL ?? (fcpStBL = new FcpStBL(Context));

        private IcmsBL icmsBL;
        public IcmsBL IcmsBL => icmsBL ?? (icmsBL = new IcmsBL(Context));

        private IbptNcmBL ibptNcmBL;
        public IbptNcmBL IbptNcmBL => ibptNcmBL ?? (ibptNcmBL = new IbptNcmBL(Context, EstadoBL));

        private IpiBL ipiBL;
        public IpiBL IpiBL => ipiBL ?? (ipiBL = new IpiBL(Context, NcmBL));

        private NcmBL ncmBL;
        public NcmBL NcmBL => ncmBL ?? (ncmBL = new NcmBL(Context));

        private SubstituicaoTributariaBL substituicaoTributariaBL;
        public SubstituicaoTributariaBL SubstituicaoTributariaBL => substituicaoTributariaBL ?? (substituicaoTributariaBL = new SubstituicaoTributariaBL(Context));

        private TabelaIcmsBL tabelaIcmsBL;
        public TabelaIcmsBL TabelaIcmsBL => tabelaIcmsBL ?? (tabelaIcmsBL = new TabelaIcmsBL(Context));
        
        private TributacaoBL tributacaoBL;
        public TributacaoBL TributacaoBL => tributacaoBL ?? (tributacaoBL = new TributacaoBL(Context, TabelaIcmsBL, NcmBL, IcmsBL, DifalBL, SubstituicaoTributariaBL, IpiBL, FcpBL, FcpStBL));


        #endregion

        #region BLS da integração com o TSS
        private AmbienteBL ambienteBL;
        public AmbienteBL AmbienteBL => ambienteBL ?? (ambienteBL = new AmbienteBL(Context, EntidadeBL));

        private CancelarFaixaBL cancelarFaixaBL;
        public CancelarFaixaBL CancelarFaixaBL => cancelarFaixaBL ?? (cancelarFaixaBL = new CancelarFaixaBL(Context, EntidadeBL));

        private InutilizarNFBL inutilizarNFBL;
        public InutilizarNFBL InutilizarNFBL => inutilizarNFBL ?? (inutilizarNFBL = new InutilizarNFBL(Context, EntidadeBL, EstadoBL, EmpresaBL));

        private CceBL cceBL;
        public CceBL CceBL => cceBL ?? (cceBL = new CceBL(Context, EntidadeBL));

        private CertificadoBL certificadoBL;
        public CertificadoBL CertificadoBL => certificadoBL ?? (certificadoBL = new CertificadoBL(Context, EntidadeBL));

        private CfopBL cfopBL;
        public CfopBL CfopBL => cfopBL ?? (cfopBL = new CfopBL(Context));

        private ChaveBL chaveBL;
        public ChaveBL ChaveBL => chaveBL ?? (chaveBL = new ChaveBL(Context, EmpresaBL, EntidadeBL, EstadoBL));

        private DanfeBL danfeBL;
        public DanfeBL DanfeBL => danfeBL ?? (danfeBL = new DanfeBL(Context, EntidadeBL));

        private EmailBL emailBL;
        public EmailBL EmailBL => emailBL ?? (emailBL = new EmailBL(Context, EntidadeBL));

        private EmpresaBL empresaBL;
        public EmpresaBL EmpresaBL => empresaBL ?? (empresaBL = new EmpresaBL(Context));

        private EmpresaIdBL empresaIdBL;
        public EmpresaIdBL EmpresaIdBL => empresaIdBL ?? (empresaIdBL = new EmpresaIdBL(Context, EmpresaBL));

        private EntidadeBL entidadeBL;
        public EntidadeBL EntidadeBL => entidadeBL ?? (entidadeBL = new EntidadeBL(Context));

        private ModalidadeBL modalidadeBL;
        public ModalidadeBL ModalidadeBL => modalidadeBL ?? (modalidadeBL = new ModalidadeBL(Context, EntidadeBL));

        private MonitorBL monitorBL;
        public MonitorBL MonitorBL => monitorBL ?? (monitorBL = new MonitorBL(Context, EntidadeBL));

        private MonitorNFSBL monitorNFSBL;
        public MonitorNFSBL MonitorNFSBL => monitorNFSBL ?? (monitorNFSBL = new MonitorNFSBL(Context, EntidadeBL));

        private NFeBL nFeBL;
        public NFeBL NFeBL => nFeBL ?? (nFeBL = new NFeBL(Context));

        private ParametroNfBL parametroNfBL;
        public ParametroNfBL ParametroNfBL => parametroNfBL ?? (parametroNfBL = new ParametroNfBL(Context, EntidadeBL));

        private RegimeTributarioBL regimeTributarioBL;
        public RegimeTributarioBL RegimeTributarioBL => regimeTributarioBL ?? (regimeTributarioBL = new RegimeTributarioBL(Context, EntidadeBL));
        
        private TransmissaoBL transmissaoBL;
        public TransmissaoBL TransmissaoBL => transmissaoBL ?? (transmissaoBL = new TransmissaoBL(Context, CfopBL, ChaveBL, CidadeBL, EmpresaBL, EntidadeBL, EstadoBL, NFeBL));

        private TransmissaoNFSBL transmissaoNFSBL;
        public TransmissaoNFSBL TransmissaoNFSBL => transmissaoNFSBL ?? (transmissaoNFSBL = new TransmissaoNFSBL(Context));

        private VersaoBL versaoBL;
        public VersaoBL VersaoBL => versaoBL ?? (versaoBL = new VersaoBL(Context, EntidadeBL));

        private MonitorEventoBL monitorEventoBL;
        public MonitorEventoBL MonitorEventoBL => monitorEventoBL ?? (monitorEventoBL = new MonitorEventoBL(Context, EntidadeBL));

        private CartaCorrecaoBL cartaCorrecaoBL;
        public CartaCorrecaoBL CartaCorrecaoBL => cartaCorrecaoBL ?? (cartaCorrecaoBL = new CartaCorrecaoBL(Context, EntidadeBL));

        #endregion
    }
}
