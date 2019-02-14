namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalAddCentroCusto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "CentroCustoId", c => c.Guid());
            CreateIndex("dbo.NotaFiscal", "CentroCustoId");
            AddForeignKey("dbo.NotaFiscal", "CentroCustoId", "dbo.CentroCusto", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscal", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.NotaFiscal", new[] { "CentroCustoId" });
            DropColumn("dbo.NotaFiscal", "CentroCustoId");
        }
    }
}
