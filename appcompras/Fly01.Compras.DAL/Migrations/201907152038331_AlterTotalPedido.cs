namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterTotalPedido : DbMigration
    {
        public override void Up()
        {
            Sql(@"Update dbo.OrdemCompra
                Set Total = 0
                Where Total is null");
            AlterColumn("dbo.OrdemCompra", "Total", c => c.Double(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemCompra", "Total", c => c.Double());
        }
    }
}
