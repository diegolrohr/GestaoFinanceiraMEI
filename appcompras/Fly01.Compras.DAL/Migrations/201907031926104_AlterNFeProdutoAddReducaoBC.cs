namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNFeProdutoAddReducaoBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.NFeProdutoEntrada", "PercentualReducaoBC", c => c.Double(nullable: false));
            AddColumn("dbo.NFeProdutoEntrada", "PercentualReducaoBCST", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.NFeProdutoEntrada", "PercentualReducaoBCST");
            DropColumn("dbo.NFeProdutoEntrada", "PercentualReducaoBC");
        }
    }
}
