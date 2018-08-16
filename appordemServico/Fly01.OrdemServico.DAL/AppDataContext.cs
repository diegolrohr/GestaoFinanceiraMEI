﻿using Fly01.Core.Base;
using Fly01.Core.BL;
using Fly01.Core.Entities.Domains.Commons;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Fly01.OrdemServico.API.Models.DAL
{
    public class AppDataContext : AppDataContextBase
    {
        public AppDataContext(ContextInitialize initialize) : base("OrdemServicoConnection")
        {
            AppUser = initialize.AppUser;
            PlataformaUrl = initialize.PlataformaUrl;
        }

        public AppDataContext() : base("OrdemServicoConnection")
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

            builder.Entity<OrdemServicoItem>()
                .Map(m => m.ToTable("OrdemServicoItem"))
                .Map<OrdemServicoItemProduto>(m => m.ToTable("OrdemServicoItemProduto"))
                .Map<OrdemServicoItemServico>(m => m.ToTable("OrdemServicoItemServico"));

            builder.Entity<Pessoa>().Ignore(m => m.CidadeCodigoIbge);
            builder.Entity<Pessoa>().Ignore(m => m.EstadoCodigoIbge);
        }

        public DbSet<GrupoProduto> GruposProduto { get; set; }

        public DbSet<Pessoa> Pessoas { get; set; }

        public DbSet<Produto> Produtos { get; set; }

        public DbSet<Core.Entities.Domains.Commons.OrdemServico> OrdensServico { get; set; }
        public DbSet<OrdemServicoItem> OrdemServicoItens { get; set; }
        public DbSet<OrdemServicoItemProduto> OrdemServicoItensProduto { get; set; }
        public DbSet<OrdemServicoItemServico> OrdemServicoItensServico { get; set; }

        public DbSet<ParametroOrdemServico> ParametrosOrdemServico { get; set; }

        public DbSet<Servico> Servicos { get; set; }

        public DbSet<UnidadeMedida> UnidadeMedidas { get; set; }
    }
}