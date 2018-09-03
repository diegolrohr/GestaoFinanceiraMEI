namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaNotaFiscalContaFinanceiraParcelaPaiIdProdutosServicos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiIdProdutos", c => c.Guid());
            AddColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiIdServicos", c => c.Guid());
            AddColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiIdProdutos", c => c.Guid());
            AddColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiIdServicos", c => c.Guid());
            DropColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiId");
            DropColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiId", c => c.Guid());
            AddColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiId", c => c.Guid());
            DropColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiIdServicos");
            DropColumn("dbo.OrdemVenda", "ContaFinanceiraParcelaPaiIdProdutos");
            DropColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiIdServicos");
            DropColumn("dbo.NotaFiscal", "ContaFinanceiraParcelaPaiIdProdutos");
        }
    }
}
