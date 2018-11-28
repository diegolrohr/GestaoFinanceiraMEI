using Fly01.Core.Entities.Domains.Commons;
using Fly01.Core.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fly01.Core.BL;

namespace Fly01.Faturamento.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("FaturamentoConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("FaturamentoConnection")
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

            builder.Entity<OrdemVendaItem>()
                .Map(m => m.ToTable("OrdemVendaItem"))
                .Map<OrdemVendaProduto>(m => m.ToTable("OrdemVendaProduto"))
                .Map<OrdemVendaServico>(m => m.ToTable("OrdemVendaServico"));

            builder.Entity<NotaFiscal>()
                .Map(m => m.ToTable("NotaFiscal"))
                .Map<NFe>(m => m.ToTable("NFe"))
                .Map<NFSe>(m => m.ToTable("NFSe"));

            builder.Entity<NotaFiscalItem>()
                .Map(m => m.ToTable("NotaFiscalItem"))
                .Map<NFeProduto>(m => m.ToTable("NFeProduto"))
                .Map<NFSeServico>(m => m.ToTable("NFSeServico"));

            builder.Entity<Pessoa>().Ignore(m => m.CidadeCodigoIbge);
            builder.Entity<Pessoa>().Ignore(m => m.EstadoCodigoIbge);
            builder.Entity<OrdemVendaItem>().Ignore(m => m.Total);
            builder.Entity<NotaFiscalItem>().Ignore(m => m.Total);
            builder.Entity<Produto>().Ignore(m => m.CodigoNcm);
            builder.Entity<Produto>().Ignore(m => m.CodigoCest);
            builder.Entity<Produto>().Ignore(m => m.AbreviacaoUnidadeMedida);
            builder.Entity<Produto>().Ignore(m => m.CodigoEnquadramentoLegalIPI);
            builder.Entity<GrupoTributario>().Ignore(m => m.CodigoCfop);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.EstadoOrigemCodigoIbge);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.EstadoDestinoCodigoIbge);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.CodigoNcm);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.CodigoCest);
            builder.Entity<Servico>().Ignore(m => m.CodigoIss);
            builder.Entity<Servico>().Ignore(m => m.CodigoNbs);
            builder.Entity<Servico>().Ignore(m => m.AbreviacaoUnidadeMedida);

        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<GrupoProduto> GruposProduto { get; set; }
        public DbSet<GrupoTributario> GrupoTributarios { get; set; }
        public DbSet<Ncm> Ncms { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<UnidadeMedida> UnidadeMedidas { get; set; }
        public DbSet<Cfop> Cfops { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Iss> Isss { get; set; }
        public DbSet<Nbs> Nbss { get; set; }
        public DbSet<Arquivo> Arquivo { get; set; }
        public DbSet<FormaPagamento> FormasPagamento { get; set; }
        public DbSet<CondicaoParcelamento> CondicoesParcelamento { get; set; }
        public DbSet<OrdemVendaItem> OrdemVendaItens { get; set; }
        public DbSet<OrdemVenda> OrdemVendas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<OrdemVendaProduto> OrdemVendaProdutos { get; set; }
        public DbSet<OrdemVendaServico> OrdemVendaServicos { get; set; }
        public DbSet<Cest> Cests { get; set; }
        public DbSet<SubstituicaoTributaria> SubstituicaoTributarias { get; set; }
        public DbSet<ParametroTributario> ParametroTributarios { get; set; }
        public DbSet<NFe> NFes { get; set; }
        public DbSet<NFeProduto> NFeProdutos { get; set; }
        public DbSet<NFSe> NFSes { get; set; }
        public DbSet<NFSeServico> NFSeServicos{ get; set; }
        public DbSet<SerieNotaFiscal> SerieNotaFiscais { get; set; }
        public DbSet<CertificadoDigital> CertificadosDigitais { get; set; }
        public DbSet<NotaFiscalItemTributacao> NotaFiscalItemTributacoes { get; set; }
        public DbSet<EnquadramentoLegalIPI> EnquadramentoLegalIPIs { get; set; }
        public DbSet<NotaFiscalInutilizada> NotaFiscalInutilizadas{ get; set; }
        public DbSet<NotaFiscalCartaCorrecao> NotaFiscalCartasCorrecao { get; set; }
    }
}