namespace Fly01.Estoque.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterInventarioItem_SaldoAtual_To_SaldoInventariado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InventarioItem", "SaldoInventariado", c => c.Double(nullable: false));
            DropColumn("dbo.InventarioItem", "SaldoAtual");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InventarioItem", "SaldoAtual", c => c.Double(nullable: false));
            DropColumn("dbo.InventarioItem", "SaldoInventariado");
        }
    }
}
