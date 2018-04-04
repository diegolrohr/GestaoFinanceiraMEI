namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterPedidoTipoFrete : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "TipoFrete", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pedido", "TipoFrete");
        }
    }
}
