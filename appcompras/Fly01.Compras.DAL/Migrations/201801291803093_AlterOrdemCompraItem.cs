namespace Fly01.Compras.DAL.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlterOrdemCompraItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrdemCompraItem", "Observacao", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrdemCompraItem", "Observacao");
        }
    }
}
