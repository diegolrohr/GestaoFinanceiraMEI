namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ProdutoTipoProduto : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Produto", "TipoProdutoId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Produto", "TipoProdutoId", c => c.Int(nullable: false));
        }
    }
}
