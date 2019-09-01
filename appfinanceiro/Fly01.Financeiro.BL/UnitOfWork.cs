using Fly01.Core.Base;
using Fly01.Core.Entities.Domains;
using Fly01.Financeiro.API.Models.DAL;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Fly01.Financeiro.BL
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

        private ContaBancariaBL contaBancariaBL;
        public ContaBancariaBL ContaBancariaBL => contaBancariaBL ?? (contaBancariaBL = new ContaBancariaBL(Context, SaldoHistoricoBL, BancoBL));

        private CategoriaBL categoriaBL;
        public CategoriaBL CategoriaBL => categoriaBL ?? (categoriaBL = new CategoriaBL(Context, ContaFinanceiraBL));

        private BancoBL bancoBL;
        public BancoBL BancoBL => bancoBL ?? (bancoBL = new BancoBL(Context));

        private ContaPagarBL contaPagarBL;
        public ContaPagarBL ContaPagarBL => contaPagarBL ?? (contaPagarBL = new ContaPagarBL(Context, CondicaoParcelamentoBL, ContaFinanceiraBaixaBL, PessoaBL));

        private ContaReceberBL contaReceberBL;
        public ContaReceberBL ContaReceberBL => contaReceberBL ?? (contaReceberBL = new ContaReceberBL(Context, CondicaoParcelamentoBL, ContaFinanceiraBaixaBL, PessoaBL));

        private EstadoBL estadoBL;
        public EstadoBL EstadoBL => estadoBL ?? (estadoBL = new EstadoBL(Context));

        private CidadeBL cidadeBL;
        public CidadeBL CidadeBL => cidadeBL ?? (cidadeBL = new CidadeBL(Context));

        private CondicaoParcelamentoBL condicaoParcelamentoBL;
        public CondicaoParcelamentoBL CondicaoParcelamentoBL => condicaoParcelamentoBL ?? (condicaoParcelamentoBL = new CondicaoParcelamentoBL(Context));

        private ContaFinanceiraBL contaFinanceiraBL;
        public ContaFinanceiraBL ContaFinanceiraBL => contaFinanceiraBL ?? (contaFinanceiraBL = new ContaFinanceiraBL(Context, SaldoHistoricoBL));

        private SaldoHistoricoBL saldoHistoricoBL;
        public SaldoHistoricoBL SaldoHistoricoBL => saldoHistoricoBL ?? (saldoHistoricoBL = new SaldoHistoricoBL(Context));

        private MovimentacaoBL movimentacaoBL;
        public MovimentacaoBL MovimentacaoBL => movimentacaoBL ?? (movimentacaoBL = new MovimentacaoBL(Context, CategoriaBL, SaldoHistoricoBL));

        private ContaFinanceiraBaixaBL contaFinanceiraBaixaBL;
        public ContaFinanceiraBaixaBL ContaFinanceiraBaixaBL => contaFinanceiraBaixaBL ?? (contaFinanceiraBaixaBL = new ContaFinanceiraBaixaBL(Context, ContaFinanceiraBL, ContaBancariaBL, SaldoHistoricoBL, MovimentacaoBL, BancoBL));

        private ContaFinanceiraBaixaMultiplaBL contaFinanceiraBaixaMultiplaBL;
        public ContaFinanceiraBaixaMultiplaBL ContaFinanceiraBaixaMultiplaBL => contaFinanceiraBaixaMultiplaBL ?? (contaFinanceiraBaixaMultiplaBL = new ContaFinanceiraBaixaMultiplaBL(Context, ContaFinanceiraBaixaBL, ContaFinanceiraBL));

        //private DemonstrativoResultadoExercicioBL demonstrativoResultadoExercicioBL;
        //public DemonstrativoResultadoExercicioBL DemonstrativoResultadoExercicioBL => demonstrativoResultadoExercicioBL ?? (demonstrativoResultadoExercicioBL = new DemonstrativoResultadoExercicioBL(Context, ContaPagarBL, ContaReceberBL, CategoriaBL));

        private ConciliacaoBancariaBL conciliacaoBancariaBL;
        public ConciliacaoBancariaBL ConciliacaoBancariaBL => conciliacaoBancariaBL ?? (conciliacaoBancariaBL = new ConciliacaoBancariaBL(Context, ConciliacaoBancariaItemBL, ContaBancariaBL));

        private ConciliacaoBancariaItemBL conciliacaoBancariaItemBL;
        public ConciliacaoBancariaItemBL ConciliacaoBancariaItemBL => conciliacaoBancariaItemBL ?? (conciliacaoBancariaItemBL = new ConciliacaoBancariaItemBL(Context));

        private ConciliacaoBancariaItemContaFinanceiraBL conciliacaoBancariaItemContaFinanceiraBL;
        public ConciliacaoBancariaItemContaFinanceiraBL ConciliacaoBancariaItemContaFinanceiraBL => conciliacaoBancariaItemContaFinanceiraBL ?? (conciliacaoBancariaItemContaFinanceiraBL = new ConciliacaoBancariaItemContaFinanceiraBL(Context, ConciliacaoBancariaBL, ConciliacaoBancariaItemBL, ContaFinanceiraBaixaBL, ContaFinanceiraBL));

        private ConciliacaoBancariaTransacaoBL conciliacaoBancariaTransacaoBL;
        public ConciliacaoBancariaTransacaoBL ConciliacaoBancariaTransacaoBL => conciliacaoBancariaTransacaoBL ?? (conciliacaoBancariaTransacaoBL = new ConciliacaoBancariaTransacaoBL(Context, ConciliacaoBancariaItemContaFinanceiraBL, ContaPagarBL, ContaReceberBL, CondicaoParcelamentoBL, ConciliacaoBancariaItemBL, ConciliacaoBancariaBL));

        private ConciliacaoBancariaBuscarExistentesBL conciliacaoBancariaBuscarExistentesBL;
        public ConciliacaoBancariaBuscarExistentesBL ConciliacaoBancariaBuscarExistentesBL => conciliacaoBancariaBuscarExistentesBL ?? (conciliacaoBancariaBuscarExistentesBL = new ConciliacaoBancariaBuscarExistentesBL(Context, ConciliacaoBancariaItemContaFinanceiraBL));

        private FormaPagamentoBL formaPagamentoBL;
        public FormaPagamentoBL FormaPagamentoBL => formaPagamentoBL ?? (formaPagamentoBL = new FormaPagamentoBL(Context));

        private ExtratoBL extratoBL;
        public ExtratoBL ExtratoBL => extratoBL ?? (extratoBL = new ExtratoBL(Context, SaldoHistoricoBL, ContaBancariaBL, MovimentacaoBL));

        private FluxoCaixaBL fluxoCaixaBL;
        public FluxoCaixaBL FluxoCaixaBL => fluxoCaixaBL ?? (fluxoCaixaBL = new FluxoCaixaBL(Context, SaldoHistoricoBL, ContaFinanceiraBL));

        private ContaFinanceiraRenegociacaoBL contaFinanceiraRenegociacaoBL;
        public ContaFinanceiraRenegociacaoBL ContaFinanceiraRenegociacaoBL => contaFinanceiraRenegociacaoBL ?? (contaFinanceiraRenegociacaoBL = new ContaFinanceiraRenegociacaoBL(Context, RenegociacaoContaFinanceiraOrigemBL, RenegociacaoContaFinanceiraRenegociadaBL, ContaPagarBL, ContaReceberBL));

        private RenegociacaoContaFinanceiraOrigemBL renegociacaoContaFinanceiraOrigemBL;
        public RenegociacaoContaFinanceiraOrigemBL RenegociacaoContaFinanceiraOrigemBL => renegociacaoContaFinanceiraOrigemBL ?? (renegociacaoContaFinanceiraOrigemBL = new RenegociacaoContaFinanceiraOrigemBL(Context));

        private RenegociacaoContaFinanceiraRenegociadaBL renegociacaoContaFinanceiraRenegociadaBL;
        public RenegociacaoContaFinanceiraRenegociadaBL RenegociacaoContaFinanceiraRenegociadaBL => renegociacaoContaFinanceiraRenegociadaBL ?? (renegociacaoContaFinanceiraRenegociadaBL = new RenegociacaoContaFinanceiraRenegociadaBL(Context));

        private TransferenciaBL transferenciaBL;
        public TransferenciaBL TransferenciaBL => transferenciaBL ?? (transferenciaBL = new TransferenciaBL(Context, MovimentacaoBL));

        private ReceitaPorCategoriaBL receitaPorCategoriaBL;
        public ReceitaPorCategoriaBL ReceitaPorCategoriaBL => receitaPorCategoriaBL ?? (receitaPorCategoriaBL = new ReceitaPorCategoriaBL(ContaReceberBL, CategoriaBL));

        private DespesaPorCategoriaBL despesaPorCategoriaBL;
        public DespesaPorCategoriaBL DespesaPorCategoriaBL => despesaPorCategoriaBL ?? (despesaPorCategoriaBL = new DespesaPorCategoriaBL(ContaPagarBL, CategoriaBL));

        private MovimentacaoPorCategoriaBL movimentacaoPorCategoriaBL;
        public MovimentacaoPorCategoriaBL MovimentacaoPorCategoriaBL => movimentacaoPorCategoriaBL ?? (movimentacaoPorCategoriaBL = new MovimentacaoPorCategoriaBL(ReceitaPorCategoriaBL, DespesaPorCategoriaBL));

        private ConfiguracaoNotificacaoBL configuracaoNotificacaoBL;
        public ConfiguracaoNotificacaoBL ConfiguracaoNotificacaoBL => configuracaoNotificacaoBL ?? (configuracaoNotificacaoBL = new ConfiguracaoNotificacaoBL(Context));
        
        private DashboardBL dashboardBL;
        public DashboardBL DashboardBL => dashboardBL ?? (dashboardBL = new DashboardBL(Context, ContaFinanceiraBL, ContaFinanceiraBaixaBL));

        private StoneAntecipacaoRecebiveisBL stoneAntecipacaoRecebiveisBL;
        public StoneAntecipacaoRecebiveisBL StoneAntecipacaoRecebiveisBL => stoneAntecipacaoRecebiveisBL ?? (stoneAntecipacaoRecebiveisBL = new StoneAntecipacaoRecebiveisBL(Context));

        private StoneBL stoneBL;
        public StoneBL StoneBL => stoneBL ?? (stoneBL = new StoneBL(Context));

        private CentroCustoBL centroCustoBL;
        public CentroCustoBL CentroCustoBL => centroCustoBL ?? (centroCustoBL = new CentroCustoBL(Context));

        private PaisBL paisBL;
        public PaisBL PaisBL => paisBL ?? (paisBL = new PaisBL(Context));

        private ConfiguracaoPersonalizacaoBL configuracaoPersonalizacaoBL;
        public ConfiguracaoPersonalizacaoBL ConfiguracaoPersonalizacaoBL => configuracaoPersonalizacaoBL ?? (configuracaoPersonalizacaoBL = new ConfiguracaoPersonalizacaoBL(Context));

        private RollbackFinanceiroCompraVendaBL rollbackFinanceiroCompraVendaBL;
        public RollbackFinanceiroCompraVendaBL RollbackFinanceiroCompraVendaBL => rollbackFinanceiroCompraVendaBL ?? (rollbackFinanceiroCompraVendaBL = new RollbackFinanceiroCompraVendaBL(Context, ContaReceberBL, ContaPagarBL, RenegociacaoContaFinanceiraOrigemBL, ContaFinanceiraRenegociacaoBL));
        
        #endregion
    }
}