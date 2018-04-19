namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemVendaNotaFiscal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFe", "TotalImpostosProdutos", c => c.Double(nullable: false));
            AddColumn("dbo.NFSe", "TotalImpostosServicos", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVenda", "TotalImpostosServicos", c => c.Double());
            AddColumn("dbo.OrdemVenda", "TotalImpostosProdutos", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVenda", "TotalImpostosProdutos");
            DropColumn("dbo.OrdemVenda", "TotalImpostosServicos");
            DropColumn("dbo.NFSe", "TotalImpostosServicos");
            DropColumn("dbo.NFe", "TotalImpostosProdutos");
        }
    }
}
