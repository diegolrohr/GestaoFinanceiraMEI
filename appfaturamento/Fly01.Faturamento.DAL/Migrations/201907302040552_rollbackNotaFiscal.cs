namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rollbackNotaFiscal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVenda", "RollbackMovimentaEstoque", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrdemVenda", "RollbackGeraFinanceiro", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "RollbackGeraFinanceiro");
            DropColumn("dbo.OrdemVenda", "RollbackMovimentaEstoque");
        }
    }
}
