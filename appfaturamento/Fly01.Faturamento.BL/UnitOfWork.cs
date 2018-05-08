﻿using Fly01.Faturamento.DAL;
using Fly01.Core.Base;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Fly01.Core.Entities.Domains;

namespace Fly01.Faturamento.BL
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

        private ProdutoBL produtoBL;
        public ProdutoBL ProdutoBL => produtoBL ?? (produtoBL = new ProdutoBL(Context, GrupoProdutoBL));

        private NCMBL ncmBL;
        public NCMBL NCMBL => ncmBL ?? (ncmBL = new NCMBL(Context));

        private NBSBL nbsBL;
        public NBSBL NBSBL => nbsBL ?? (nbsBL = new NBSBL(Context));

        private GrupoProdutoBL grupoProdutoBL;
        public GrupoProdutoBL GrupoProdutoBL => grupoProdutoBL ?? (grupoProdutoBL = new GrupoProdutoBL(Context));

        private UnidadeMedidaBL unidadeMedidaBL;
        public UnidadeMedidaBL UnidadeMedidaBL => unidadeMedidaBL ?? (unidadeMedidaBL = new UnidadeMedidaBL(Context));

        private CfopBL cfopBL;
        public CfopBL CfopBL => cfopBL ?? (cfopBL = new CfopBL(Context));

        private GrupoTributarioBL grupoTributarioBL;
        public GrupoTributarioBL GrupoTributarioBL => grupoTributarioBL ?? (grupoTributarioBL = new GrupoTributarioBL(Context));

        private OrdemVendaBL ordemVendaBL;
        public OrdemVendaBL OrdemVendaBL => ordemVendaBL ?? (ordemVendaBL = new OrdemVendaBL(Context, OrdemVendaProdutoBL, OrdemVendaServicoBL, NFeBL, NFSeBL, NFeProdutoBL, NFSeServicoBL, TotalTributacaoBL, NotaFiscalItemTributacaoBL));

        private OrdemVendaProdutoBL ordemVendaProdutoBL;
        public OrdemVendaProdutoBL OrdemVendaProdutoBL => ordemVendaProdutoBL ?? (ordemVendaProdutoBL = new OrdemVendaProdutoBL(Context));

        private OrdemVendaServicoBL ordemVendaServicoBL;
        public OrdemVendaServicoBL OrdemVendaServicoBL => ordemVendaServicoBL ?? (ordemVendaServicoBL = new OrdemVendaServicoBL(Context));

        private ServicoBL servicoBL;
        public ServicoBL ServicoBL => servicoBL ?? (servicoBL = new ServicoBL(Context));

        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        public CondicaoParcelamentoBL CondicaoParcelamentoBL => condicaoParcelamentoBL ?? (condicaoParcelamentoBL = new CondicaoParcelamentoBL(Context));

        private FormaPagamentoBL formaPagamentoBL;
        public FormaPagamentoBL FormaPagamentoBL => formaPagamentoBL ?? (formaPagamentoBL = new FormaPagamentoBL(Context));

        private CategoriaBL categoriaBL;
        public CategoriaBL CategoriaBL => categoriaBL ?? (categoriaBL = new CategoriaBL(Context, OrdemVendaBL));

        private SubstituicaoTributariaBL substituicaoTributariaBL;
        public SubstituicaoTributariaBL SubstituicaoTributariaBL => substituicaoTributariaBL ?? (substituicaoTributariaBL = new SubstituicaoTributariaBL(Context));

        private CestBL cestBL;
        public CestBL CestBL => cestBL ?? (cestBL = new CestBL(Context));

        private ParametroTributarioBL parametroTributarioBL;
        public ParametroTributarioBL ParametroTributarioBL => parametroTributarioBL ?? (parametroTributarioBL = new ParametroTributarioBL(Context, EntidadeBL));

        private NotaFiscalBL notaFiscalBL;
        public NotaFiscalBL NotaFiscalBL => notaFiscalBL ?? (notaFiscalBL = new NotaFiscalBL(Context, NFeBL, NFSeBL, CertificadoDigitalBL, TotalTributacaoBL));

        private NFeBL nfeBL;
        public NFeBL NFeBL => nfeBL ?? (nfeBL = new NFeBL(Context, SerieNotaFiscalBL, NFeProdutoBL, TotalTributacaoBL, CertificadoDigitalBL, PessoaBL, CondicaoParcelamentoBL, SubstituicaoTributariaBL, NotaFiscalItemTributacaoBL, FormaPagamentoBL));

        private NFeProdutoBL nfeProdutoBL;
        public NFeProdutoBL NFeProdutoBL => nfeProdutoBL ?? (nfeProdutoBL = new NFeProdutoBL(Context));

        private NFSeBL nfseBL;
        public NFSeBL NFSeBL => nfseBL ?? (nfseBL = new NFSeBL(Context, SerieNotaFiscalBL, NFSeServicoBL, TotalTributacaoBL));

        private NFSeServicoBL nfseServicoBL;
        public NFSeServicoBL NFSeServicoBL => nfseServicoBL ?? (nfseServicoBL = new NFSeServicoBL(Context));

        private SerieNotaFiscalBL serieNotaFiscalBL;
        public SerieNotaFiscalBL SerieNotaFiscalBL => serieNotaFiscalBL ?? (serieNotaFiscalBL = new SerieNotaFiscalBL(Context));

        private SerieNotaFiscalInutilizadaBL serieNotaFiscalInutilizadaBL;
        public SerieNotaFiscalInutilizadaBL SerieNotaFiscalInutilizadaBL => serieNotaFiscalInutilizadaBL ?? (serieNotaFiscalInutilizadaBL = new SerieNotaFiscalInutilizadaBL(Context));

        private CertificadoDigitalBL certificadoDigitalBL;
        public CertificadoDigitalBL CertificadoDigitalBL => certificadoDigitalBL ?? (certificadoDigitalBL = new CertificadoDigitalBL(Context, EstadoBL, ParametroTributarioBL));
        
        private TotalTributacaoBL totalTributacaoBL;
        public TotalTributacaoBL TotalTributacaoBL => totalTributacaoBL ?? (totalTributacaoBL = new TotalTributacaoBL(Context, PessoaBL, GrupoTributarioBL, ProdutoBL, SubstituicaoTributariaBL, ParametroTributarioBL, CertificadoDigitalBL));

        private MonitorNFBL monitorNFBL;
        public MonitorNFBL MonitorNFBL => monitorNFBL ?? (monitorNFBL = new MonitorNFBL(Context, TotalTributacaoBL, NFeBL, NFSeBL, NotaFiscalBL, CertificadoDigitalBL));

        private EntidadeBL entidadeBL;
        public EntidadeBL EntidadeBL => entidadeBL ?? (entidadeBL = new EntidadeBL(Context, EstadoBL));

        private NotaFiscalItemTributacaoBL notaFiscalItemTributacaoBL;
        public NotaFiscalItemTributacaoBL NotaFiscalItemTributacaoBL => notaFiscalItemTributacaoBL ?? (notaFiscalItemTributacaoBL = new NotaFiscalItemTributacaoBL(Context));

        private EnquadramentoLegalIPIBL enquadramentoLegalIPIBL;
        public EnquadramentoLegalIPIBL EnquadramentoLegalIPIBL => enquadramentoLegalIPIBL ?? (enquadramentoLegalIPIBL = new EnquadramentoLegalIPIBL(Context));
        #endregion
    }
}
