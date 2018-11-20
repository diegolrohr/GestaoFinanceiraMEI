namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterOrdemVendaProdutoImpostos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemVendaProduto", "Icms", c => c.Double(nullable: false));
            AddColumn("dbo.OrdemVendaProduto", "Fcp", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemVendaProduto", "Fcp");
            DropColumn("dbo.OrdemVendaProduto", "Icms");
        }
    }
}
