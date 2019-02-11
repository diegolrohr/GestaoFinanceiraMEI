namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalPedidoOrigem : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.NotaFiscal", new[] { "OrdemVendaOrigemId" });
            AlterColumn("dbo.NotaFiscal", "OrdemVendaOrigemId", c => c.Guid());
            CreateIndex("dbo.NotaFiscal", "OrdemVendaOrigemId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotaFiscal", new[] { "OrdemVendaOrigemId" });
            AlterColumn("dbo.NotaFiscal", "OrdemVendaOrigemId", c => c.Guid(nullable: false));
            CreateIndex("dbo.NotaFiscal", "OrdemVendaOrigemId");
        }
    }
}
