namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterGeraNotaFiscal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pedido", "GeraNotaFiscal", c => c.Boolean(nullable: false));
            DropColumn("dbo.OrdemCompra", "GeraNotaFiscal");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrdemCompra", "GeraNotaFiscal", c => c.Boolean(nullable: false));
            DropColumn("dbo.Pedido", "GeraNotaFiscal");
        }
    }
}
