namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemCompra : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.OrdemCompra", new[] { "GrupoTributarioPadraoId" });
            AlterColumn("dbo.OrdemCompra", "GrupoTributarioPadraoId", c => c.Guid());
            CreateIndex("dbo.OrdemCompra", "GrupoTributarioPadraoId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.OrdemCompra", new[] { "GrupoTributarioPadraoId" });
            AlterColumn("dbo.OrdemCompra", "GrupoTributarioPadraoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OrdemCompra", "GrupoTributarioPadraoId");
        }
    }
}
