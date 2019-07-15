namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterPedidoItemAddPercentualReducaoBC : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PedidoItem", "PercentualReducaoBC", c => c.Double(nullable: false));
            AddColumn("dbo.PedidoItem", "PercentualReducaoBCST", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PedidoItem", "PercentualReducaoBCST");
            DropColumn("dbo.PedidoItem", "PercentualReducaoBC");
        }
    }
}
