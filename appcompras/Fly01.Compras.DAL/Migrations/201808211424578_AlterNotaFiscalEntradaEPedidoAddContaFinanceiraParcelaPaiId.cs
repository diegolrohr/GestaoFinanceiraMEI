namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNotaFiscalEntradaEPedidoAddContaFinanceiraParcelaPaiId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscalEntrada", "GeraFinanceiro", c => c.Boolean(nullable: false));
            AddColumn("dbo.NotaFiscalEntrada", "ContaFinanceiraParcelaPaiId", c => c.Guid());
            AddColumn("dbo.Pedido", "ContaFinanceiraParcelaPaiId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "ContaFinanceiraParcelaPaiId");
            DropColumn("dbo.NotaFiscalEntrada", "ContaFinanceiraParcelaPaiId");
            DropColumn("dbo.NotaFiscalEntrada", "GeraFinanceiro");
        }
    }
}
