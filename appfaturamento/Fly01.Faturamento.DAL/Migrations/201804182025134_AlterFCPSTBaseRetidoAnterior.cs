namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterFCPSTBaseRetidoAnterior : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeProduto", "ValorBCFCPSTRetidoAnterior", c => c.Double(nullable: false, defaultValue: 0));
            AddColumn("dbo.OrdemVendaProduto", "ValorBCFCPSTRetidoAnterior", c => c.Double(nullable: false, defaultValue: 0));
            DropColumn("dbo.NFeProduto", "AliquotaFCPConsumidorFinal");
            DropColumn("dbo.OrdemVendaProduto", "AliquotaFCPConsumidorFinal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemVendaProduto", "AliquotaFCPConsumidorFinal", c => c.Double(nullable: false));
            AddColumn("dbo.NFeProduto", "AliquotaFCPConsumidorFinal", c => c.Double(nullable: false));
            DropColumn("dbo.OrdemVendaProduto", "ValorBCFCPSTRetidoAnterior");
            DropColumn("dbo.NFeProduto", "ValorBCFCPSTRetidoAnterior");
        }
    }
}
