namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalEntradaPedidoOrigem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotaFiscalEntrada", new[] { "OrdemCompraOrigemId" });
            AlterColumn("dbo.NotaFiscalEntrada", "OrdemCompraOrigemId", c => c.Guid());
            CreateIndex("dbo.NotaFiscalEntrada", "OrdemCompraOrigemId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotaFiscalEntrada", new[] { "OrdemCompraOrigemId" });
            AlterColumn("dbo.NotaFiscalEntrada", "OrdemCompraOrigemId", c => c.Guid(nullable: false));
            CreateIndex("dbo.NotaFiscalEntrada", "OrdemCompraOrigemId");
        }
    }
}
