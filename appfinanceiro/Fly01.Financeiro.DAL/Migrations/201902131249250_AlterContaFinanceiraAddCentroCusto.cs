namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterContaFinanceiraAddCentroCusto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaFinanceira", "CentroCustoId", c => c.Guid());
            CreateIndex("dbo.ContaFinanceira", "CentroCustoId");
            AddForeignKey("dbo.ContaFinanceira", "CentroCustoId", "dbo.CentroCusto", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContaFinanceira", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.ContaFinanceira", new[] { "CentroCustoId" });
            DropColumn("dbo.ContaFinanceira", "CentroCustoId");
        }
    }
}
