namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterProduto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Produto", "SaldoProduto", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produto", "SaldoProduto", c => c.Double());
        }
    }
}
