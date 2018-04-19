namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemVendaENotaImpostosRetidos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeProduto", "ValorCreditoICMS", c => c.Double());
            AddColumn("dbo.NFeProduto", "ValorICMSSTRetido", c => c.Double());
            AddColumn("dbo.NFeProduto", "ValorBCSTRetido", c => c.Double());
            AddColumn("dbo.OrdemVendaProduto", "ValorCreditoICMS", c => c.Double());
            AddColumn("dbo.OrdemVendaProduto", "ValorICMSSTRetido", c => c.Double());
            AddColumn("dbo.OrdemVendaProduto", "ValorBCSTRetido", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVendaProduto", "ValorBCSTRetido");
            DropColumn("dbo.OrdemVendaProduto", "ValorICMSSTRetido");
            DropColumn("dbo.OrdemVendaProduto", "ValorCreditoICMS");
            DropColumn("dbo.NFeProduto", "ValorBCSTRetido");
            DropColumn("dbo.NFeProduto", "ValorICMSSTRetido");
            DropColumn("dbo.NFeProduto", "ValorCreditoICMS");
        }
    }
}
