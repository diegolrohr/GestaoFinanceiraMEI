using Fly01.EmissaoNFE.Domain;
using Fly01.Core.Base;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using Fly01.EmissaoNFE.Domain.Entities.NFe.IBPT;

namespace Fly01.EmissaoNFE.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("EmissaoNFEConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("EmissaoNFEConnection")
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
        
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<TabelaIcms> TabelaIcms { get; set; }
        public DbSet<Ncm> Ncms { get; set; }
        public DbSet<Nbs> Nbss { get; set; }
        public DbSet<Cfop> Cfops { get; set; }
        public DbSet<IbptNcm> IbptNcms { get; set; }
        public DbSet<Siafi> Siafis { get; set; }

    }
}
