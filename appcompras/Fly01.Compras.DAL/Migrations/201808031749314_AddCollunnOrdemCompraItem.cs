namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCollunnOrdemCompraItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemCompraItem", "GrupoTributarioId", c => c.Guid(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorCreditoICMS", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorICMSSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorBCSTRetido", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorFCPSTRetidoAnterior", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemCompraItem", "ValorBCFCPSTRetidoAnterior", c => c.Double(nullable: false));
            CreateIndex("dbo.OrdemCompraItem", "GrupoTributarioId");
            AddForeignKey("dbo.OrdemCompraItem", "GrupoTributarioId", "dbo.GrupoTributario", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdemCompraItem", "GrupoTributarioId", "dbo.GrupoTributario");
            DropIndex("dbo.OrdemCompraItem", new[] { "GrupoTributarioId" });
            DropColumn("dbo.OrdemCompraItem", "ValorBCFCPSTRetidoAnterior");
            DropColumn("dbo.OrdemCompraItem", "ValorFCPSTRetidoAnterior");
            DropColumn("dbo.OrdemCompraItem", "ValorBCSTRetido");
            DropColumn("dbo.OrdemCompraItem", "ValorICMSSTRetido");
            DropColumn("dbo.OrdemCompraItem", "ValorCreditoICMS");
            DropColumn("dbo.OrdemCompraItem", "GrupoTributarioId");
        }
    }
}
