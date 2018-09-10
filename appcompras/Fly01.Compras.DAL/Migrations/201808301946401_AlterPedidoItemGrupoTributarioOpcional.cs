namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPedidoItemGrupoTributarioOpcional : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.PedidoItem", new[] { "GrupoTributarioId" });
            AlterColumn("dbo.PedidoItem", "GrupoTributarioId", c => c.Guid());
            CreateIndex("dbo.PedidoItem", "GrupoTributarioId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.PedidoItem", new[] { "GrupoTributarioId" });
            AlterColumn("dbo.PedidoItem", "GrupoTributarioId", c => c.Guid(nullable: false));
            CreateIndex("dbo.PedidoItem", "GrupoTributarioId");
        }
    }
}
