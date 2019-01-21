namespace Fly01.Faturamento.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateImageProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "ImageProduto", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produto", "ImageProduto");
        }
    }
}
