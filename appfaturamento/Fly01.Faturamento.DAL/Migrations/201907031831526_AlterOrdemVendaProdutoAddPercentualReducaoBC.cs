namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaProdutoAddPercentualReducaoBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVendaProduto", "PercentualReducaoBC", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVendaProduto", "PercentualReducaoBCST", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVendaProduto", "PercentualReducaoBCST");
            DropColumn("dbo.OrdemVendaProduto", "PercentualReducaoBC");
        }
    }
}
