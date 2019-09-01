using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fly01.Core.Base;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Financeiro.API.Models.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("GestaoFinanceiraMEIConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("GestaoFinanceiraMEIConnection")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Conventions.Remove<PluralizingTableNameConvention>();
            builder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            builder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            builder.Properties()
                .Where(p => p.Name == "Id")
                .Configure(p => p.IsKey());

            builder.Properties<string>()
                .Configure(x => x.HasMaxLength(200));

            builder.Properties<string>()
                .Configure(x => x.HasColumnType("varchar"));

            builder.Entity<ContaFinanceira>()
                .Map(m => m.ToTable("ContaFinanceira"))
                .Map<ContaPagar>(m => m.ToTable("ContaPagar"))
                .Map<ContaReceber>(m => m.ToTable("ContaReceber"));

            //What does principal end of an association means in relationship
            //por causa da repeticaoPaiId, explicito via FluentAPI
            builder.Entity<ContaFinanceira>()
                .HasOptional(b => b.ContaFinanceiraParcelaPai)
                .WithMany()
                .HasForeignKey(b => b.ContaFinanceiraParcelaPaiId)
                .WillCascadeOnDelete(false);

            builder.Entity<RenegociacaoContaFinanceira>()
                .Map(m => m.ToTable("RenegociacaoContaFinanceira"))
                .Map<RenegociacaoContaFinanceiraOrigem>(m => m.ToTable("RenegociacaoContaFinanceiraOrigem"))
                .Map<RenegociacaoContaFinanceiraRenegociada>(m => m.ToTable("RenegociacaoContaFinanceiraRenegociada"));

            builder.Entity<ContaFinanceiraRenegociacao>().Ignore(m => m.ContasFinanceirasOrigemIds);
            builder.Entity<ContaFinanceira>().Ignore(m => m.Saldo);
            builder.Entity<ContaFinanceira>().Ignore(m => m.NomePessoa);
            builder.Entity<ContaFinanceira>().Ignore(m => m.ContaBancariaId);
            builder.Entity<ContaFinanceira>().Ignore(m => m.ContaBancaria);
            builder.Entity<ConciliacaoBancaria>().Ignore(m => m.Arquivo);
            builder.Entity<ContaBancaria>().Ignore(m => m.CodigoBanco);
            builder.Entity<Pessoa>().Ignore(m => m.CidadeCodigoIbge);
            builder.Entity<Pessoa>().Ignore(m => m.EstadoCodigoIbge);
            builder.Entity<SaldoHistorico>().MapToStoredProcedures();
            //builder.Entity<ContaFinanceira>().MapToStoredProcedures();
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<ContaBancaria> ContasBancarias { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<CondicaoParcelamento> CondicoesParcelamento { get; set; }
        public DbSet<ContaFinanceira> ContasFinanceiras { get; set; }
        public DbSet<ContaPagar> ContasPagar { get; set; }
        public DbSet<ContaReceber> ContasReceber { get; set; }
        public DbSet<ContaFinanceiraBaixa> ContasFinanceirasBaixas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<SaldoHistorico> SaldosHistorico { get; set; }
        public DbSet<ConciliacaoBancaria> ConciliacoesBancarias { get; set; }
        public DbSet<ConciliacaoBancariaItem> ConciliacaoBancariaItens { get; set; }
        public DbSet<ConciliacaoBancariaItemContaFinanceira> ConciliacaoBancariaItemContasFinanceiras { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<MovimentacaoFinanceira> Movimentacao { get; set; }
        public DbSet<ContaFinanceiraRenegociacao> ContasFinanceirasRenegociacoes { get; set; }
        public DbSet<RenegociacaoContaFinanceiraOrigem> RenegociacaoContasFinanceirasOrigem { get; set; }
        public DbSet<RenegociacaoContaFinanceiraRenegociada> RenegociacaoContasFinanceirasRenegociadas { get; set; }
        public DbSet<StoneAntecipacaoRecebiveis> StoneAntecipacaoRecebiveis { get; set; }
        public DbSet<Pais> Paises { get; set; }
        public DbSet<ConfiguracaoPersonalizacao> ConfiguracaoPersonalizacoes { get; set; }
    }
}