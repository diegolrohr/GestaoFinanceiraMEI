namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalEOrdemVendaAddContaFinanceiraParcelaPaiId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "GeraFinanceiro", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiId", c => c.Guid());
            AddColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiId");
            DropColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiId");
            DropColumn("dbo.NotaFiscal", "GeraFinanceiro");
        }
    }
}
