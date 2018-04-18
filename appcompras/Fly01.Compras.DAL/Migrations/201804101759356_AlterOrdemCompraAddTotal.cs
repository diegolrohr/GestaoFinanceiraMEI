namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemCompraAddTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemCompra", "Total", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemCompra", "Total");
        }
    }
}
