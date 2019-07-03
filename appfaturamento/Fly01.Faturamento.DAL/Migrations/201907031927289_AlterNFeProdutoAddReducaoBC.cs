namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeProdutoAddReducaoBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeProduto", "PercentualReducaoBC", c => c.Double(nullable: false));
            AddColumn("dbo.NFeProduto", "PercentualReducaoBCST", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFeProduto", "PercentualReducaoBCST");
            DropColumn("dbo.NFeProduto", "PercentualReducaoBC");
        }
    }
}
