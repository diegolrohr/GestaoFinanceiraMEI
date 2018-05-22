namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterPlataformaBaseRegistroFixo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GrupoProduto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.InventarioItem", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Inventario", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Produto", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.Movimento", "RegistroFixo", c => c.Boolean(nullable: false));
            AddColumn("dbo.TipoMovimento", "RegistroFixo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TipoMovimento", "RegistroFixo");
            DropColumn("dbo.Movimento", "RegistroFixo");
            DropColumn("dbo.Produto", "RegistroFixo");
            DropColumn("dbo.Inventario", "RegistroFixo");
            DropColumn("dbo.InventarioItem", "RegistroFixo");
            DropColumn("dbo.GrupoProduto", "RegistroFixo");
        }
    }
}
