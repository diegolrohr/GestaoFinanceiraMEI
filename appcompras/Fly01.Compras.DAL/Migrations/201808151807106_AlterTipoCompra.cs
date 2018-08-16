namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTipoCompra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "TipoCompra", c => c.Int(nullable: false));
            DropColumn("dbo.OrdemCompra", "TipoCompra");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemCompra", "TipoCompra", c => c.Int(nullable: false));
            DropColumn("dbo.Pedido", "TipoCompra");
        }
    }
}
