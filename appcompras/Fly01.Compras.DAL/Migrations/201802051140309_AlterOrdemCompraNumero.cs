namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemCompraNumero : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemCompra", "Numero", c => c.Int(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemCompra", "Numero");
        }
    }
}
