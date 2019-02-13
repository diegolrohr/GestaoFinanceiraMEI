namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemCompraNotaFiscalAddCentroCusto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalEntrada", "CentroCustoId", c => c.Guid());
            AddColumn("dbo.OrdemCompra", "CentroCustoId", c => c.Guid());
            AddColumn("dbo.NFeImportacao", "CentroCustoId", c => c.Guid());
            CreateIndex("dbo.NotaFiscalEntrada", "CentroCustoId");
            CreateIndex("dbo.OrdemCompra", "CentroCustoId");
            CreateIndex("dbo.NFeImportacao", "CentroCustoId");
            AddForeignKey("dbo.OrdemCompra", "CentroCustoId", "dbo.CentroCusto", "Id");
            AddForeignKey("dbo.NFeImportacao", "CentroCustoId", "dbo.CentroCusto", "Id");
            AddForeignKey("dbo.NotaFiscalEntrada", "CentroCustoId", "dbo.CentroCusto", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalEntrada", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.NFeImportacao", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.OrdemCompra", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.NFeImportacao", new[] { "CentroCustoId" });
            DropIndex("dbo.OrdemCompra", new[] { "CentroCustoId" });
            DropIndex("dbo.NotaFiscalEntrada", new[] { "CentroCustoId" });
            DropColumn("dbo.NFeImportacao", "CentroCustoId");
            DropColumn("dbo.OrdemCompra", "CentroCustoId");
            DropColumn("dbo.NotaFiscalEntrada", "CentroCustoId");
        }
    }
}
