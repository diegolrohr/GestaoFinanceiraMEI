namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ALterContaFinanceiraAddContaFinanceiraParcelaPaiId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId", c => c.Guid());
            CreateIndex("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId");
            AddForeignKey("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId", "dbo.ContaFinanceira", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId", "dbo.ContaFinanceira");
            DropIndex("dbo.ContaFinanceira", new[] { "ContaFinanceiraParcelaPaiId" });
            DropColumn("dbo.ContaFinanceira", "ContaFinanceiraParcelaPaiId");
        }
    }
}
