namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaAddCentroCusto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "CentroCustoId", c => c.Guid());
            CreateIndex("dbo.OrdemVenda", "CentroCustoId");
            AddForeignKey("dbo.OrdemVenda", "CentroCustoId", "dbo.CentroCusto", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrdemVenda", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.OrdemVenda", new[] { "CentroCustoId" });
            DropColumn("dbo.OrdemVenda", "CentroCustoId");
        }
    }
}
