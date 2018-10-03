namespace Fly01.Faturamento.DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaNotaFiscalImpostosServicos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFSe", "TotalRetencoesServicos", c => c.Double(nullable: false));
            AddColumn("dbo.NFSe", "TotalImpostosServicosNaoAgrega", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVenda", "TotalRetencoesServicos", c => c.Double());
            AddColumn("dbo.OrdemVenda", "TotalImpostosServicosNaoAgrega", c => c.Double(nullable: false));
            DropColumn("dbo.NFSe", "TotalImpostosServicos");
            DropColumn("dbo.OrdemVenda", "TotalImpostosServicos");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemVenda", "TotalImpostosServicos", c => c.Double());
            AddColumn("dbo.NFSe", "TotalImpostosServicos", c => c.Double(nullable: false));
            DropColumn("dbo.OrdemVenda", "TotalImpostosServicosNaoAgrega");
            DropColumn("dbo.OrdemVenda", "TotalRetencoesServicos");
            DropColumn("dbo.NFSe", "TotalImpostosServicosNaoAgrega");
            DropColumn("dbo.NFSe", "TotalRetencoesServicos");
        }
    }
}
