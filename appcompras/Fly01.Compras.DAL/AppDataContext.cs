using Fly01.Core;
using Fly01.Core.Base;
using System.Data.Entity;
using Fly01.Compras.Domain.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

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

            builder.Entity<OrdemCompraItem>().Ignore(m => m.Total);
        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<CondicaoParcelamento> CondicoesParcelamento { get; set; }
        public DbSet<FormaPagamento> FormaPagamentos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<NCM> Ncms { get; set; }
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
    }
}
