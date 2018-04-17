using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fly01.Estoque.Domain.Entities;
using Fly01.Core.Base;
using Fly01.Core.BL;

namespace Fly01.Estoque.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("EstoqueConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("EstoqueConnection")
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
        }

        public DbSet<Estado> Estados { get; set; }
        public DbSet<NCM> Ncms { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<UnidadeMedida> UnidadeMedidas { get; set; }
        public DbSet<GrupoProduto> GruposProduto { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<InventarioItem> InventarioItens { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<TipoMovimento> TiposMovimento { get; set; }
        public DbSet<Cest> Cests { get; set; }
        public DbSet<EnquadramentoLegalIPI> EnquadramentoLegalIPIs { get; set; }
    }
}