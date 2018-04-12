namespace Fly01.Compras.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterNumeroOrdemCompra : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrdemCompra", "Numero", c => c.Int(nullable: false));
            AddColumn("dbo.OrdemCompra", "NumeroAux", c => c.Int(nullable: false));
            Sql("update ordemcompra set NumeroAux = Numero");
            DropColumn("dbo.OrdemCompra", "Numero");
            RenameColumn(table: "dbo.OrdemCompra", name: "NumeroAux", newName: "Numero");

        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrdemCompra", "Numero", c => c.Int(nullable: false, identity: true));
        }
    }
}
