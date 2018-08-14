namespace Fly01.EmissaoNFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTabelaICMSDeleteColumnsEstadosIds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TabelaIcms", "EstadoDestinoId", "dbo.Estado");
            DropForeignKey("dbo.TabelaIcms", "EstadoOrigemId", "dbo.Estado");
            DropIndex("dbo.TabelaIcms", new[] { "EstadoOrigemId" });
            DropIndex("dbo.TabelaIcms", new[] { "EstadoDestinoId" });
            DropColumn("dbo.TabelaIcms", "EstadoOrigemId");
            DropColumn("dbo.TabelaIcms", "EstadoDestinoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TabelaIcms", "EstadoDestinoId", c => c.Guid(nullable: false));
            AddColumn("dbo.TabelaIcms", "EstadoOrigemId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TabelaIcms", "EstadoDestinoId");
            CreateIndex("dbo.TabelaIcms", "EstadoOrigemId");
            AddForeignKey("dbo.TabelaIcms", "EstadoOrigemId", "dbo.Estado", "Id");
            AddForeignKey("dbo.TabelaIcms", "EstadoDestinoId", "dbo.Estado", "Id");
        }
    }
}
