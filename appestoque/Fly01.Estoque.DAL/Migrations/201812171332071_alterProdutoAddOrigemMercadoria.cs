namespace Fly01.Estoque.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterProdutoAddOrigemMercadoria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "OrigemMercadoria", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produto", "OrigemMercadoria");
        }
    }
}
