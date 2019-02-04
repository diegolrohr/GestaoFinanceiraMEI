using Fly01.Core.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;

namespace Fly01.Compras.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("ComprasConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("ComprasConnection")
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

            builder.Entity<OrdemCompra>()
                .Map(m => m.ToTable("OrdemCompra"))
                .Map<Orcamento>(m => m.ToTable("Orcamento"))
                .Map<Pedido>(m => m.ToTable("Pedido"));

            builder.Entity<OrdemCompraItem>()
                .Map(m => m.ToTable("OrdemCompraItem"))
                .Map<OrcamentoItem>(m => m.ToTable("OrcamentoItem"))
                .Map<PedidoItem>(m => m.ToTable("PedidoItem"));

            builder.Entity<Pessoa>().Ignore(m => m.CidadeCodigoIbge);
            builder.Entity<Pessoa>().Ignore(m => m.EstadoCodigoIbge);
            builder.Entity<OrdemCompraItem>().Ignore(m => m.Total);
            builder.Entity<Produto>().Ignore(m => m.CodigoNcm);
            builder.Entity<Produto>().Ignore(m => m.CodigoCest);
            builder.Entity<Produto>().Ignore(m => m.AbreviacaoUnidadeMedida);
            builder.Entity<Produto>().Ignore(m => m.CodigoEnquadramentoLegalIPI);
            builder.Entity<GrupoTributario>().Ignore(m => m.CodigoCfop);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.EstadoOrigemCodigoIbge);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.EstadoDestinoCodigoIbge);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.CodigoNcm);
            builder.Entity<SubstituicaoTributaria>().Ignore(m => m.CodigoCest);
            builder.Entity<NotaFiscalEntrada>().Ignore(m => m.EstadoCodigoIbge);
            builder.Entity<OrdemCompra>().Ignore(m => m.EstadoCodigoIbge);

            builder.Entity<NotaFiscalEntrada>()
                .Map(m => m.ToTable("NotaFiscalEntrada"))
                .Map<NFeEntrada>(m => m.ToTable("NFeEntrada"));

            builder.Entity<NotaFiscalItemEntrada>()
                .Map(m => m.ToTable("NotaFiscalItemEntrada"))
                .Map<NFeProdutoEntrada>(m => m.ToTable("NFeProdutoEntrada"));

            #region One-to-Zero-or-One Relationship Two way FK
            builder.Entity<NFeImportacao>()
              .HasOptional(s => s.Pedido)
              .WithMany()
              .HasForeignKey(s => s.PedidoId);

            builder.Entity<NFeImportacaoProduto>()
              .HasOptional(s => s.PedidoItem)
              .WithMany()
              .HasForeignKey(s => s.PedidoItemId);

            builder.Entity<Pedido>()
              .HasOptional(s => s.NFeImportacao)
              .WithMany()
              .HasForeignKey(s => s.NFeImportacaoId);

            builder.Entity<PedidoItem>()
              .HasOptional(s => s.NFeImportacaoProduto)              
              .WithMany()
              .HasForeignKey(s => s.NFeImportacaoProdutoId);
            #endregion
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<CondicaoParcelamento> CondicoesParcelamento { get; set; }
        public DbSet<FormaPagamento> FormaPagamentos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Ncm> Ncms { get; set; }
        public DbSet<UnidadeMedida> UnidadeMedidas { get; set; }
        public DbSet<GrupoProduto> GruposProduto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Orcamento> Orcamentos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<SubstituicaoTributaria> SubstituicaoTributarias { get; set; }
        public DbSet<Cfop> Cfops { get; set; }
        public DbSet<GrupoTributario> GrupoTributarios { get; set; }
        public DbSet<Cest> Cests { get; set; }
        public DbSet<Arquivo> Arquivo { get; set; }
        public DbSet<EnquadramentoLegalIPI> EnquadramentoLegalIPIs { get; set; }
        public DbSet<CertificadoDigital> CertificadoDigitais { get; set; }
        public DbSet<ParametroTributario> ParametroTributarios { get; set; }
        public DbSet<SerieNotaFiscal> SerieNotaFiscais { get; set; }
        public DbSet<NotaFiscalInutilizada> NotaFiscalInutilizadas { get; set; }
        public DbSet<NotaFiscalItemTributacaoEntrada> NotaFiscalItemTributacoesEntrada { get; set; }
        public DbSet<NotaFiscalCartaCorrecaoEntrada> NotaFiscalCartasCorrecaoEntrada { get; set; }
        public DbSet<NFeEntrada> NFeEntradas { get; set; }
        public DbSet<NFeProdutoEntrada> NFeProdutoEntradas { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Iss> Isss { get; set; }
        public DbSet<Nbs> Nbss { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<KitItem> KitItens { get; set; }
        public DbSet<NFeImportacao> NFeImportacoes { get; set; }
        public DbSet<NFeImportacaoProduto> NFeImportacaoProdutos { get; set; }
        public DbSet<NFeImportacaoCobranca> NFeImportacaoCobrancas { get; set; }
    }
}
