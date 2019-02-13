namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterContaFinanceiraRenegociacaoAddCentroCusto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaFinanceiraRenegociacao", "CentroCustoId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ContaFinanceiraRenegociacao", "CentroCustoId");
        }
    }
}
