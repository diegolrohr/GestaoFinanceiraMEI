namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddTipoProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produto", "TipoProduto", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produto", "TipoProduto");
        }
    }
}
