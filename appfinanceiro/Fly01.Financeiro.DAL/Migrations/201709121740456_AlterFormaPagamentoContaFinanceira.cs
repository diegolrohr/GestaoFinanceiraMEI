namespace Fly01.Financeiro.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterFormaPagamentoContaFinanceira : DbMigration
    {
        public override void Up()
        {
            if(DateTime.Now.Date < (new DateTime(2017, 09, 25)))
            {
                Sql("Delete contaPagar");//tirar para prod
                Sql("Delete contaReceber");
                Sql("Delete contaFinanceira");
            }
            AddColumn("dbo.ContaFinanceira", "FormaPagamentoId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ContaFinanceira", "FormaPagamentoId");
            AddForeignKey("dbo.ContaFinanceira", "FormaPagamentoId", "dbo.FormaPagamento", "Id");
            DropColumn("dbo.ContaFinanceira", "FormaPagamento");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ContaFinanceira", "FormaPagamento", c => c.Int(nullable: false));
            DropForeignKey("dbo.ContaFinanceira", "FormaPagamentoId", "dbo.FormaPagamento");
            DropIndex("dbo.ContaFinanceira", new[] { "FormaPagamentoId" });
            DropColumn("dbo.ContaFinanceira", "FormaPagamentoId");
        }
    }
}
