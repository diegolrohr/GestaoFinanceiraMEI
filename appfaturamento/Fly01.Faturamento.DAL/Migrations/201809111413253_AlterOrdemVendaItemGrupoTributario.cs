namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaItemGrupoTributario : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrdemVendaItem", new[] { "GrupoTributarioId" });
            AlterColumn("dbo.OrdemVendaItem", "GrupoTributarioId", c => c.Guid());
            CreateIndex("dbo.OrdemVendaItem", "GrupoTributarioId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrdemVendaItem", new[] { "GrupoTributarioId" });
            AlterColumn("dbo.OrdemVendaItem", "GrupoTributarioId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrdemVendaItem", "GrupoTributarioId");
        }
    }
}
